using Moq;
using Xunit;
using System.Collections.Generic;
using Testing_LR2;
using Testing_LR2.Objects;

namespace Testing_LR2_tests;

public class ProcessManagerTests
{
    private ProcessManager processManager;
    private Mock<Controller> controllerMock;
    private Mock<SeismicSensor> seismicSensorMock;
    private Mock<DrillingRig> drillingRigMock;
    private Mock<Geologist> geologistMock;
    private Mock<DrillingEngineer> drillingEngineerMock;

    public ProcessManagerTests()
    {
        controllerMock = new Mock<Controller>();
        seismicSensorMock = new Mock<SeismicSensor>();
        drillingRigMock = new Mock<DrillingRig>();
        geologistMock = new Mock<Geologist>();
        drillingEngineerMock = new Mock<DrillingEngineer>(drillingRigMock.Object);

        processManager = new ProcessManager(
            controllerMock.Object,
            seismicSensorMock.Object,
            drillingRigMock.Object,
            geologistMock.Object,
            drillingEngineerMock.Object
        );
    }

    // 1. Успешное выполнение всего процесса
    [Fact]
    public void Sequence1_FullProcess_SuccessfullyCompletes()
    {
        // Arrange
        controllerMock.Setup(c => c.CheckState()).Returns("Включен");
        seismicSensorMock.Setup(s => s.CheckState()).Returns("В режиме ожидания");
        seismicSensorMock.Setup(s => s.SelfDiagnose()).Returns(true);

        geologistMock.Setup(g => g.StartWork());
        geologistMock.Setup(g => g.ActivateSeismicSensor(seismicSensorMock.Object));

        seismicSensorMock.Setup(s => s.RecordSeismicData()).Returns(new SeismicData(50.0));
        var data = new Data("SeismicSensor", 50.0);
        controllerMock.Setup(c => c.CollectData(seismicSensorMock.Object)).Returns(data);

        var processedData = new ProcessedData(new List<Data> { data });
        processedData.ProcessedValues["AverageAmplitude"] = 50.0;
        controllerMock.Setup(c => c.ProcessData(data)).Returns(processedData);

        var map = new Map();
        controllerMock.Setup(c => c.CreateMap(processedData)).Returns(map);

        drillingRigMock.Setup(r => r.CheckState()).Returns("Остановлена");
        drillingEngineerMock.Setup(e => e.StartWork());
        controllerMock.Setup(c => c.SendCommand(drillingEngineerMock.Object, "Запустить бурение"));

        var sample = new OilSample();
        drillingRigMock.Setup(r => r.ExtractSample()).Returns(sample);

        var analysis = new Analysis("1", new Dictionary<string, double> { { "Нефть", 50.0 } });
        geologistMock.Setup(g => g.AnalyzeSample(sample)).Returns(analysis);

        var report = new Report();
        controllerMock.Setup(c => c.CreateReport(analysis)).Returns(report);

        // Act
        var controllerState = processManager.CheckControllerState();
        Assert.Equal("Включен", controllerState);

        var sensorState = processManager.CheckSeismicSensorState();
        Assert.Equal("В режиме ожидания", sensorState);

        var sensorDiagnostics = processManager.CheckSeismicSensorDiagnostics();
        Assert.True(sensorDiagnostics);

        processManager.ActivateSeismicSensor();
        var collectedData = processManager.CollectData();
        var processed = processManager.ProcessData(collectedData);
        var createdMap = processManager.BuildMap(processed);

        var rigState = processManager.CheckDrillingRigState();
        Assert.Equal("Остановлена", rigState);

        processManager.StartDrilling();
        var extractedSample = processManager.ExtractSample();
        var sampleAnalysis = processManager.AnalyzeSample(extractedSample);
        var finalReport = processManager.CompileReport(sampleAnalysis);

        // Assert
        Assert.NotNull(finalReport);
        controllerMock.Verify(c => c.CheckState(), Times.Once());
        seismicSensorMock.Verify(s => s.CheckState(), Times.Once());
        seismicSensorMock.Verify(s => s.SelfDiagnose(), Times.Once());
        geologistMock.Verify(g => g.StartWork(), Times.Once());
        geologistMock.Verify(g => g.ActivateSeismicSensor(seismicSensorMock.Object), Times.Once());
        seismicSensorMock.Verify(s => s.RecordSeismicData(), Times.Once());
        controllerMock.Verify(c => c.CollectData(seismicSensorMock.Object), Times.Once());
        controllerMock.Verify(c => c.ProcessData(It.IsAny<Data>()), Times.Once());
        controllerMock.Verify(c => c.CreateMap(It.IsAny<ProcessedData>()), Times.Once());
        drillingRigMock.Verify(r => r.CheckState(), Times.Once());
        drillingEngineerMock.Verify(e => e.StartWork(), Times.Once());
        controllerMock.Verify(c => c.SendCommand(drillingEngineerMock.Object, "Запустить бурение"), Times.Once());
        drillingRigMock.Verify(r => r.ExtractSample(), Times.Once());
        geologistMock.Verify(g => g.AnalyzeSample(It.IsAny<OilSample>()), Times.Once());
        controllerMock.Verify(c => c.CreateReport(It.IsAny<Analysis>()), Times.Once());
    }

    // 2. Проверка состояния контроллера → Сбой системы
    [Fact]
    public void Sequence2_ControllerFailure_ExitsWithSystemFailure()
    {
        // Arrange
        controllerMock.Setup(c => c.CheckState()).Returns("Ошибка");

        // Act
        var controllerState = processManager.CheckControllerState();

        // Assert
        Assert.Equal("Ошибка", controllerState);
        controllerMock.Verify(c => c.CheckState(), Times.Once());
    }

    // 3. Проверка состояния контроллера → Проверка состояния сейсмического датчика → Активация сейсмического датчика → Сбор данных → Обработка данных → Отсутствие запасов
    [Fact]
    public void Sequence3_NoReserves_ExitsWithNoReserves()
    {
        // Arrange
        controllerMock.Setup(c => c.CheckState()).Returns("Включен");
        seismicSensorMock.Setup(s => s.CheckState()).Returns("В режиме ожидания");
        seismicSensorMock.Setup(s => s.SelfDiagnose()).Returns(true);

        geologistMock.Setup(g => g.StartWork());
        geologistMock.Setup(g => g.ActivateSeismicSensor(seismicSensorMock.Object));

        seismicSensorMock.Setup(s => s.RecordSeismicData()).Returns(new SeismicData(5.0));
        var data = new Data("SeismicSensor", 5.0);
        controllerMock.Setup(c => c.CollectData(seismicSensorMock.Object)).Returns(data);

        var processedData = new ProcessedData(new List<Data> { data });
        processedData.ProcessedValues["AverageAmplitude"] = 5.0; // Меньше порога для наличия запасов
        controllerMock.Setup(c => c.ProcessData(data)).Returns(processedData);

        // Act
        var controllerState = processManager.CheckControllerState();
        Assert.Equal("Включен", controllerState);

        var sensorState = processManager.CheckSeismicSensorState();
        Assert.Equal("В режиме ожидания", sensorState);

        var sensorDiagnostics = processManager.CheckSeismicSensorDiagnostics();
        Assert.True(sensorDiagnostics);

        processManager.ActivateSeismicSensor();
        var collectedData = processManager.CollectData();
        var processed = processManager.ProcessData(collectedData);
        var hasReserves = processManager.HasReserves(processed);

        // Assert
        Assert.False(hasReserves);
        controllerMock.Verify(c => c.CheckState(), Times.Once());
        seismicSensorMock.Verify(s => s.CheckState(), Times.Once());
        seismicSensorMock.Verify(s => s.SelfDiagnose(), Times.Once());
        geologistMock.Verify(g => g.StartWork(), Times.Once());
        geologistMock.Verify(g => g.ActivateSeismicSensor(seismicSensorMock.Object), Times.Once());
        seismicSensorMock.Verify(s => s.RecordSeismicData(), Times.Once());
        controllerMock.Verify(c => c.CollectData(seismicSensorMock.Object), Times.Once());
        controllerMock.Verify(c => c.ProcessData(It.IsAny<Data>()), Times.Once());
    }

    // 4. Проверка состояния контроллера → Проверка состояния сейсмического датчика → Активация сейсмического датчика → Сбор данных → Обработка данных → Построение карты местности → Проверка состояния буровой установки → Неисправность оборудования
    [Fact]
    public void Sequence4_DrillingRigFailure_ExitsWithEquipmentFailure()
    {
        // Arrange
        controllerMock.Setup(c => c.CheckState()).Returns("Включен");
        seismicSensorMock.Setup(s => s.CheckState()).Returns("В режиме ожидания");
        seismicSensorMock.Setup(s => s.SelfDiagnose()).Returns(true);

        geologistMock.Setup(g => g.StartWork());
        geologistMock.Setup(g => g.ActivateSeismicSensor(seismicSensorMock.Object));

        seismicSensorMock.Setup(s => s.RecordSeismicData()).Returns(new SeismicData(50.0));
        var data = new Data("SeismicSensor", 50.0);
        controllerMock.Setup(c => c.CollectData(seismicSensorMock.Object)).Returns(data);

        var processedData = new ProcessedData(new List<Data> { data });
        processedData.ProcessedValues["AverageAmplitude"] = 50.0;
        controllerMock.Setup(c => c.ProcessData(data)).Returns(processedData);

        var map = new Map();
        controllerMock.Setup(c => c.CreateMap(processedData)).Returns(map);

        drillingRigMock.Setup(r => r.CheckState()).Returns("Требует ремонта");

        // Act
        var controllerState = processManager.CheckControllerState();
        Assert.Equal("Включен", controllerState);

        var sensorState = processManager.CheckSeismicSensorState();
        Assert.Equal("В режиме ожидания", sensorState);

        var sensorDiagnostics = processManager.CheckSeismicSensorDiagnostics();
        Assert.True(sensorDiagnostics);

        processManager.ActivateSeismicSensor();
        var collectedData = processManager.CollectData();
        var processed = processManager.ProcessData(collectedData);
        var createdMap = processManager.BuildMap(processed);

        var rigState = processManager.CheckDrillingRigState();
        Assert.Equal("Требует ремонта", rigState);

        // Assert
        controllerMock.Verify(c => c.CheckState(), Times.Once());
        seismicSensorMock.Verify(s => s.CheckState(), Times.Once());
        seismicSensorMock.Verify(s => s.SelfDiagnose(), Times.Once());
        geologistMock.Verify(g => g.StartWork(), Times.Once());
        geologistMock.Verify(g => g.ActivateSeismicSensor(seismicSensorMock.Object), Times.Once());
        seismicSensorMock.Verify(s => s.RecordSeismicData(), Times.Once());
        controllerMock.Verify(c => c.CollectData(seismicSensorMock.Object), Times.Once());
        controllerMock.Verify(c => c.ProcessData(It.IsAny<Data>()), Times.Once());
        controllerMock.Verify(c => c.CreateMap(It.IsAny<ProcessedData>()), Times.Once());
        drillingRigMock.Verify(r => r.CheckState(), Times.Once());
    }

    // 5. Проверка состояния контроллера → Проверка состояния сейсмического датчика → Неисправность оборудования
    [Fact]
    public void Sequence5_SeismicSensorFailure_ExitsWithEquipmentFailure()
    {
        // Arrange
        controllerMock.Setup(c => c.CheckState()).Returns("Включен");
        seismicSensorMock.Setup(s => s.CheckState()).Returns("В режиме ожидания");
        seismicSensorMock.Setup(s => s.SelfDiagnose()).Returns(false);

        // Act
        var controllerState = processManager.CheckControllerState();
        Assert.Equal("Включен", controllerState);

        var sensorState = processManager.CheckSeismicSensorState();
        Assert.Equal("В режиме ожидания", sensorState);

        var sensorDiagnostics = processManager.CheckSeismicSensorDiagnostics();
        Assert.False(sensorDiagnostics);

        // Assert
        controllerMock.Verify(c => c.CheckState(), Times.Once());
        seismicSensorMock.Verify(s => s.CheckState(), Times.Once());
        seismicSensorMock.Verify(s => s.SelfDiagnose(), Times.Once());
    }
}
using Moq;
using Xunit;
using System.Collections.Generic;
using Testing_LR2;
using Testing_LR2.Objects;

public class UseCaseTests
{
    private Mock<Controller> controllerMock;
    private Mock<SeismicSensor> seismicSensorMock;
    private Mock<DrillingRig> drillingRigMock;
    private Mock<Geologist> geologistMock;
    private Mock<DrillingEngineer> drillingEngineerMock;

    public UseCaseTests()
    {
        controllerMock = new Mock<Controller>();
        seismicSensorMock = new Mock<SeismicSensor>();
        drillingRigMock = new Mock<DrillingRig>();
        geologistMock = new Mock<Geologist>();
        drillingEngineerMock = new Mock<DrillingEngineer>(drillingRigMock.Object);
    }

    // Тест 1: Геолог активирует сейсмический датчик
    [Fact]
    public void Geologist_ActivateSeismicSensor_Success()
    {
        // Arrange
        seismicSensorMock.Setup(s => s.CheckState()).Returns("В режиме ожидания");
        seismicSensorMock.Setup(s => s.SelfDiagnose()).Returns(true);
        geologistMock.Setup(g => g.StartWork());
        geologistMock.Setup(g => g.ActivateSeismicSensor(seismicSensorMock.Object));

        // Act
        var sensorState = seismicSensorMock.Object.CheckState();
        var sensorDiagnostics = seismicSensorMock.Object.SelfDiagnose();
        geologistMock.Object.StartWork();
        geologistMock.Object.ActivateSeismicSensor(seismicSensorMock.Object);

        // Assert
        Assert.Equal("В режиме ожидания", sensorState);
        Assert.True(sensorDiagnostics);
        geologistMock.Verify(g => g.StartWork(), Times.Once());
        geologistMock.Verify(g => g.ActivateSeismicSensor(seismicSensorMock.Object), Times.Once());
    }

    // Тест 2: Геолог анализирует образец
    [Fact]
    public void Geologist_AnalyzeSample_Success()
    {
        // Arrange
        var sample = new OilSample();
        var analysis = new Analysis("1", new Dictionary<string, double> { { "Нефть", 50.0 } });
        geologistMock.Setup(g => g.AnalyzeSample(sample)).Returns(analysis);

        // Act
        var result = geologistMock.Object.AnalyzeSample(sample);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(analysis, result);
        geologistMock.Verify(g => g.AnalyzeSample(sample), Times.Once());
    }

    // Тест 3: Контроллер проверяет состояние системы (успех)
    [Fact]
    public void Controller_CheckSystemState_Success()
    {
        // Arrange
        controllerMock.Setup(c => c.CheckState()).Returns("Включен");

        // Act
        var controllerState = controllerMock.Object.CheckState();

        // Assert
        Assert.Equal("Включен", controllerState);
        controllerMock.Verify(c => c.CheckState(), Times.Once());
    }

    // Тест 4: Контроллер проверяет состояние системы (сбой системы)
    [Fact]
    public void Controller_CheckSystemState_SystemFailure()
    {
        // Arrange
        controllerMock.Setup(c => c.CheckState()).Returns("Ошибка");

        // Act
        var controllerState = controllerMock.Object.CheckState();

        // Assert
        Assert.Equal("Ошибка", controllerState);
        controllerMock.Verify(c => c.CheckState(), Times.Once());
    }

    // Тест 5: Контроллер собирает данные
    [Fact]
    public void Controller_CollectData_Success()
    {
        // Arrange
        seismicSensorMock.Setup(s => s.CheckState()).Returns("Активен");
        seismicSensorMock.Setup(s => s.SelfDiagnose()).Returns(true);
        seismicSensorMock.Setup(s => s.RecordSeismicData()).Returns(new SeismicData(50.0));
        var data = new Data("SeismicSensor", 50.0);
        controllerMock.Setup(c => c.CollectData(seismicSensorMock.Object)).Returns(data);

        // Act
        seismicSensorMock.Object.RecordSeismicData();
        var collectedData = controllerMock.Object.CollectData(seismicSensorMock.Object);

        // Assert
        Assert.NotNull(collectedData);
        Assert.Equal(data, collectedData);
        seismicSensorMock.Verify(s => s.RecordSeismicData(), Times.Once());
        controllerMock.Verify(c => c.CollectData(seismicSensorMock.Object), Times.Once());
    }

    // Тест 6: Контроллер собирает данные (неисправность оборудования)
    [Fact]
    public void Controller_CollectData_EquipmentFailure()
    {
        // Arrange
        seismicSensorMock.Setup(s => s.CheckState()).Returns("В режиме ожидания");
        seismicSensorMock.Setup(s => s.SelfDiagnose()).Returns(false);

        // Act
        var sensorState = seismicSensorMock.Object.CheckState();
        var sensorDiagnostics = seismicSensorMock.Object.SelfDiagnose();

        // Assert
        Assert.Equal("В режиме ожидания", sensorState);
        Assert.False(sensorDiagnostics);
        seismicSensorMock.Verify(s => s.CheckState(), Times.Once());
        seismicSensorMock.Verify(s => s.SelfDiagnose(), Times.Once());
    }

    // Тест 7: Контроллер обрабатывает данные и строит карту местности (успех)
    [Fact]
    public void Controller_ProcessDataAndBuildMap_Success()
    {
        // Arrange
        var data = new Data("SeismicSensor", 50.0);
        var processedData = new ProcessedData(new List<Data> { data });
        processedData.ProcessedValues["AverageAmplitude"] = 50.0;
        controllerMock.Setup(c => c.ProcessData(data)).Returns(processedData);

        var map = new Map();
        controllerMock.Setup(c => c.CreateMap(processedData)).Returns(map);

        // Act
        var processed = controllerMock.Object.ProcessData(data);
        var createdMap = controllerMock.Object.CreateMap(processed);

        // Assert
        Assert.NotNull(processed);
        Assert.NotNull(createdMap);
        controllerMock.Verify(c => c.ProcessData(data), Times.Once());
        controllerMock.Verify(c => c.CreateMap(processedData), Times.Once());
    }

    // Тест 8: Контроллер обрабатывает данные и строит карту местности (отсутствие запасов)
    [Fact]
    public void Controller_ProcessDataAndBuildMap_NoReserves()
    {
        // Arrange
        var data = new Data("SeismicSensor", 5.0);
        var processedData = new ProcessedData(new List<Data> { data });
        processedData.ProcessedValues["AverageAmplitude"] = 5.0; // Меньше порога
        controllerMock.Setup(c => c.ProcessData(data)).Returns(processedData);

        var map = new Map();
        controllerMock.Setup(c => c.CreateMap(processedData)).Returns(map);

        // Act
        var processed = controllerMock.Object.ProcessData(data);
        var createdMap = controllerMock.Object.CreateMap(processed);
        var hasReserves = processed.ProcessedValues["AverageAmplitude"] > 10; // Имитация HasReserves

        // Assert
        Assert.NotNull(processed);
        Assert.NotNull(createdMap);
        Assert.False(hasReserves);
        controllerMock.Verify(c => c.ProcessData(data), Times.Once());
        controllerMock.Verify(c => c.CreateMap(processedData), Times.Once());
    }

    // Тест 9: Контроллер запускает бурение (успех)
    [Fact]
    public void Controller_StartDrilling_Success()
    {
        // Arrange
        drillingRigMock.Setup(r => r.CheckState()).Returns("Остановлена");
        drillingEngineerMock.Setup(e => e.StartWork());
        controllerMock.Setup(c => c.SendCommand(drillingEngineerMock.Object, "Запустить бурение"));

        // Act
        var rigState = drillingRigMock.Object.CheckState();
        drillingEngineerMock.Object.StartWork();
        controllerMock.Object.SendCommand(drillingEngineerMock.Object, "Запустить бурение");

        // Assert
        Assert.Equal("Остановлена", rigState);
        drillingEngineerMock.Verify(e => e.StartWork(), Times.Once());
        controllerMock.Verify(c => c.SendCommand(drillingEngineerMock.Object, "Запустить бурение"), Times.Once());
    }

    // Тест 10: Контроллер запускает бурение (неисправность оборудования)
    [Fact]
    public void Controller_StartDrilling_EquipmentFailure()
    {
        // Arrange
        drillingRigMock.Setup(r => r.CheckState()).Returns("Требует ремонта");

        // Act
        var rigState = drillingRigMock.Object.CheckState();

        // Assert
        Assert.Equal("Требует ремонта", rigState);
        drillingRigMock.Verify(r => r.CheckState(), Times.Once());
    }

    // Тест 11: Контроллер извлекает образец
    [Fact]
    public void Controller_ExtractSample_Success()
    {
        // Arrange
        var sample = new OilSample();
        drillingRigMock.Setup(r => r.ExtractSample()).Returns(sample);

        // Act
        var extractedSample = drillingRigMock.Object.ExtractSample();

        // Assert
        Assert.NotNull(extractedSample);
        drillingRigMock.Verify(r => r.ExtractSample(), Times.Once());
    }

    // Тест 12: Контроллер составляет отчёт
    [Fact]
    public void Controller_CompileReport_Success()
    {
        // Arrange
        var analysis = new Analysis("1", new Dictionary<string, double> { { "Нефть", 50.0 } });
        var report = new Report();
        controllerMock.Setup(c => c.CreateReport(analysis)).Returns(report);

        // Act
        var finalReport = controllerMock.Object.CreateReport(analysis);

        // Assert
        Assert.NotNull(finalReport);
        controllerMock.Verify(c => c.CreateReport(analysis), Times.Once());
    }
}
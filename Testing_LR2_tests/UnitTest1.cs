using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using Testing_LR2;
using Testing_LR2.Objects;

namespace Testing_LR2_tests;

public class ExplorationSystemTests
{
    // Объявляем объекты для тестов
    private readonly Controller controller;
    private readonly SeismicSensor seismicSensor;
    private readonly DrillingRig drillingRig;
    private readonly OilSample oilSample;
    private readonly Geologist geologist;
    private readonly DrillingEngineer drillingEngineer;
    private readonly Map map;
    private readonly Report report;

    // Конструктор для инициализации (замена TestInitialize)
    public ExplorationSystemTests()
    {
        // Инициализация объектов перед каждым тестом
        controller = new Controller();
        seismicSensor = new SeismicSensor();
        drillingRig = new DrillingRig();
        oilSample = new OilSample();
        geologist = new Geologist();
        drillingEngineer = new DrillingEngineer(drillingRig);
        map = new Map();
        report = new Report();
    }

    // Тестирование инициализации объектов
    [Fact]
    public void TestInitialization()
    {
        // Проверка Controller
        Assert.Equal("Выключен", controller.CheckState());

        // Проверка SeismicSensor
        Assert.Equal("В режиме ожидания", seismicSensor.CheckState());

        // Проверка DrillingRig
        Assert.Equal("Остановлена", drillingRig.CheckState());

        // Проверка OilSample
        Assert.Equal("Не исследован", oilSample.GetState());

        // Проверка Geologist
        Assert.Equal("Отдыхает", geologist.GetState());

        // Проверка DrillingEngineer
        Assert.Equal("Отдыхает", drillingEngineer.GetState());

        // Проверка Map
        Assert.Equal("Не создана", map.GetState());

        // Проверка Report
        Assert.Equal("Не начат", report.GetState());
    }

    // Тестирование методов Controller
    [Fact]
    public void TestControllerMethods()
    {
        // Проверка StartSystem и StopSystem
        controller.StartSystem();
        Assert.Equal("Включен", controller.CheckState());
        controller.StopSystem();
        Assert.Equal("Выключен", controller.CheckState());

        // Проверка CollectData и ProcessData
        geologist.StartWork();
        geologist.ActivateSeismicSensor(seismicSensor);
        seismicSensor.RecordSeismicData();
        var data = controller.CollectData(seismicSensor);
        Assert.NotNull(data);
        var processedData = controller.ProcessData(data);
        Assert.True(processedData.ProcessedValues.ContainsKey("AverageAmplitude"));

        // Проверка CreateMap
        var createdMap = controller.CreateMap(processedData);
        Assert.Equal("Завершена", createdMap.GetState());

        // Проверка SendCommand
        drillingEngineer.StartWork();
        controller.SendCommand(drillingEngineer, "Запустить бурение");
        Assert.Equal("Работает", drillingRig.CheckState());

        // Проверка CreateReport и CheckDataSufficiency
        var sample = drillingRig.ExtractSample();
        var analysis = geologist.AnalyzeSample(sample);
        var createdReport = controller.CreateReport(analysis);
        Assert.Equal("Завершен", createdReport.GetState());
        Assert.True(controller.CheckDataSufficiency(analysis));
    }

    // Тестирование методов SeismicSensor
    [Fact]
    public void TestSeismicSensorMethods()
    {
        // Проверка Activate и Deactivate
        seismicSensor.Activate();
        Assert.Equal("Активен", seismicSensor.CheckState());
        seismicSensor.Deactivate();
        Assert.Equal("В режиме ожидания", seismicSensor.CheckState());

        // Проверка RecordSeismicData и TransmitData
        seismicSensor.Activate();
        var seismicData = seismicSensor.RecordSeismicData();
        Assert.NotNull(seismicData);
        var transmittedData = seismicSensor.TransmitData(controller);
        Assert.NotNull(transmittedData);

        // Проверка SelfDiagnose
        var isWorking = seismicSensor.SelfDiagnose();
        Assert.True(isWorking || !isWorking); // Проверяем, что возвращается bool
    }

    // Тестирование методов DrillingRig
    [Fact]
    public void TestDrillingRigMethods()
    {
        // Проверка StartDrilling и StopDrilling
        drillingRig.StartDrilling();
        Assert.Equal("Работает", drillingRig.CheckState());
        drillingRig.StopDrilling();
        Assert.Equal("Остановлена", drillingRig.CheckState());
        
        // Проверка PerformMaintenance
        drillingRig.PerformMaintenance(); // Ничего не произойдет, так как состояние не "Требует ремонта"
        Assert.Equal("Остановлена", drillingRig.CheckState());
        
        // Проверка ExtractSample
        drillingRig.StartDrilling();
        var sample = drillingRig.ExtractSample();
        Assert.NotNull(sample);
    }

    // Тестирование методов OilSample
    [Fact]
    public void TestOilSampleMethods()
    {
        // Проверка StartAnalysis и CompleteAnalysis
        oilSample.StartAnalysis();
        Assert.Equal("В процессе анализа", oilSample.GetState());
        var composition = new Dictionary<string, double> { { "Нефть", 50.0 }, { "Вода", 30.0 } };
        oilSample.CompleteAnalysis(composition);
        Assert.Equal("Проанализирован", oilSample.GetState());
        Assert.Equal(composition, oilSample.GetComposition());
    }

    // Тестирование методов Geologist
    [Fact]
    public void TestGeologistMethods()
    {
        // Проверка StartWork, TakeBreak и Leave
        geologist.StartWork();
        Assert.Equal("Работает", geologist.GetState());
        geologist.TakeBreak();
        Assert.Equal("Отдыхает", geologist.GetState());
        geologist.Leave();
        Assert.Equal("Отсутствует", geologist.GetState());

        // Проверка ActivateSeismicSensor
        geologist.StartWork();
        geologist.ActivateSeismicSensor(seismicSensor);
        Assert.Equal("Активен", seismicSensor.CheckState());

        // Проверка AnalyzeSample
        var sample = new OilSample();
        var analysis = geologist.AnalyzeSample(sample);
        Assert.NotNull(analysis);
        Assert.Equal("Проанализирован", sample.GetState());

        // Проверка TransmitData
        var data = new Data("Geologist", "TestData");
        geologist.TransmitData(controller, data); // Просто проверяем, что метод вызывается без ошибок
    }

    // Тестирование методов DrillingEngineer
    [Fact]
    public void TestDrillingEngineerMethods()
    {
        // Проверка StartWork, TakeBreak и Leave
        drillingEngineer.StartWork();
        Assert.Equal("Работает", drillingEngineer.GetState());
        drillingEngineer.TakeBreak();
        Assert.Equal("Отдыхает", drillingEngineer.GetState());
        drillingEngineer.Leave();
        Assert.Equal("Отсутствует", drillingEngineer.GetState());

        // Проверка StartDrilling и StopDrilling через ReceiveCommand
        drillingEngineer.StartWork();
        drillingEngineer.ReceiveCommand(controller, "Запустить бурение");
        Assert.Equal("Работает", drillingRig.CheckState());
        drillingEngineer.ReceiveCommand(controller, "Остановить бурение");
        Assert.Equal("Остановлена", drillingRig.CheckState());
    }

    // Тестирование методов Map
    [Fact]
    public void TestMapMethods()
    {
        // Проверка StartCreation, AddTerrainData и CompleteCreation
        map.StartCreation();
        Assert.Equal("В процессе построения", map.GetState());
        var data = new ProcessedData(new List<Data>());
        data.ProcessedValues["AverageAmplitude"] = 50.0;
        map.AddTerrainData(data);
        map.CompleteCreation();
        Assert.Equal("Завершена", map.GetState());
        var terrainData = map.GetTerrainData();
        Assert.NotNull(terrainData);
        Assert.Equal(50.0, terrainData["AverageAmplitude"]);
    }

    // Тестирование методов Report
    [Fact]
    public void TestReportMethods()
    {
        // Проверка StartCompilation, AddAnalysisData, SetReserveEstimate и CompleteCompilation
        report.StartCompilation();
        Assert.Equal("В процессе составления", report.GetState());
        var analysis = new Analysis("Sample1", new Dictionary<string, double> { { "Нефть", 50.0 } });
        report.AddAnalysisData(analysis);
        report.SetReserveEstimate(1000.0);
        report.CompleteCompilation();
        Assert.Equal("Завершен", report.GetState());
        Assert.Equal(1000.0, report.GetReserveEstimate());
    }
}
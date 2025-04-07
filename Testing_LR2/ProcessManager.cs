using Testing_LR2.Objects;

namespace Testing_LR2;

public class ProcessManager
{
    private readonly Controller controller;
    private readonly SeismicSensor seismicSensor;
    private readonly DrillingRig drillingRig;
    private readonly Geologist geologist;
    private readonly DrillingEngineer drillingEngineer;

    public ProcessManager(Controller controller, SeismicSensor seismicSensor, DrillingRig drillingRig, Geologist geologist, DrillingEngineer drillingEngineer)
    {
        this.controller = controller;
        this.seismicSensor = seismicSensor;
        this.drillingRig = drillingRig;
        this.geologist = geologist;
        this.drillingEngineer = drillingEngineer;
    }

    public string CheckControllerState()
    {
        return controller.CheckState();
    }

    public string CheckSeismicSensorState()
    {
        return seismicSensor.CheckState();
    }

    public bool CheckSeismicSensorDiagnostics()
    {
        return seismicSensor.SelfDiagnose();
    }

    public void ActivateSeismicSensor()
    {
        geologist.StartWork();
        geologist.ActivateSeismicSensor(seismicSensor);
    }

    public Data CollectData()
    {
        seismicSensor.RecordSeismicData();
        return controller.CollectData(seismicSensor);
    }

    public ProcessedData ProcessData(Data data)
    {
        return controller.ProcessData(data);
    }

    public Map BuildMap(ProcessedData data)
    {
        return controller.CreateMap(data);
    }

    public string CheckDrillingRigState()
    {
        return drillingRig.CheckState();
    }

    public void StartDrilling()
    {
        drillingEngineer.StartWork();
        controller.SendCommand(drillingEngineer, "Запустить бурение");
    }

    public OilSample ExtractSample()
    {
        return drillingRig.ExtractSample();
    }

    public Analysis AnalyzeSample(OilSample sample)
    {
        return geologist.AnalyzeSample(sample);
    }

    public Report CompileReport(Analysis analysis)
    {
        return controller.CreateReport(analysis);
    }

    public bool HasReserves(ProcessedData data)
    {
        // Имитация проверки наличия запасов (в реальном коде это может быть сложная логика)
        return data.ProcessedValues.ContainsKey("AverageAmplitude") && data.ProcessedValues["AverageAmplitude"] > 10;
    }
}
namespace Testing_LR2.Objects;

public class Controller
{
    // Поля
    public static string state;
    public static List<Data> collectedData;
    public static List<object> connectedObjects;

    // Конструктор
    public Controller()
    {
        state = "Выключен";
        collectedData = new List<Data>();
        connectedObjects = new List<object>();
    }

    // Методы
    public virtual void StartSystem()
    {
        state = "Включен";
        Console.WriteLine("Система запущена.");
    }

    public virtual void StopSystem()
    {
        state = "Выключен";
        Console.WriteLine("Система остановлена.");
    }

    public virtual string CheckState()
    {
        return state;
    }

    public virtual Data CollectData(object obj)
    {
        if (obj is SeismicSensor sensor)
        {
            var data = sensor.TransmitData(this);
            collectedData.Add(data);
            return data;
        }
        return null;
    }

    public virtual void SendCommand(object obj, string command)
    {
        if (obj is DrillingEngineer engineer)
        {
            engineer.ReceiveCommand(this, command);
        }
        Console.WriteLine($"Команда '{command}' отправлена объекту {obj.GetType().Name}.");
    }

    public virtual ProcessedData ProcessData(Data data)
    {
        var processed = new ProcessedData(new List<Data> { data });
        processed.ProcessedValues["AverageAmplitude"] = (double)data.Value;
        return processed;
    }

    public virtual Map CreateMap(ProcessedData data)
    {
        var map = new Map();
        map.StartCreation();
        map.AddTerrainData(data);
        map.CompleteCreation();
        return map;
    }

    public virtual Report CreateReport(Analysis analysis)
    {
        var report = new Report();
        report.StartCompilation();
        report.AddAnalysisData(analysis);
        report.SetReserveEstimate(1000.0); // Пример значения
        report.CompleteCompilation();
        return report;
    }

    public virtual bool CheckDataSufficiency(Analysis analysis)
    {
        return analysis.Composition.ContainsKey("Нефть") && analysis.Composition["Нефть"] > 0;
    }
}
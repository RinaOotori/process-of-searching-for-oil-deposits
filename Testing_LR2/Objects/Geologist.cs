namespace Testing_LR2.Objects;

public class Geologist
{
    // Поля
    private string state;
    private List<OilSample> analyzedSamples;
    private List<SeismicData> seismicData;

    // Конструктор
    public Geologist()
    {
        state = "Отдыхает";
        analyzedSamples = new List<OilSample>();
        seismicData = new List<SeismicData>();
    }

    // Методы
    public void StartWork()
    {
        state = "Работает";
        Console.WriteLine("Геолог начал работу.");
    }

    public void TakeBreak()
    {
        state = "Отдыхает";
        Console.WriteLine("Геолог отдыхает.");
    }

    public void Leave()
    {
        state = "Отсутствует";
        Console.WriteLine("Геолог отсутствует.");
    }

    public void ActivateSeismicSensor(SeismicSensor sensor)
    {
        if (state == "Работает")
        {
            sensor.Activate();
        }
    }

    public Analysis AnalyzeSample(OilSample sample)
    {
        if (state == "Работает")
        {
            sample.StartAnalysis();
            var composition = new Dictionary<string, double>
            {
                { "Нефть", new Random().NextDouble() * 100 },
                { "Вода", new Random().NextDouble() * 50 }
            };
            sample.CompleteAnalysis(composition);
            analyzedSamples.Add(sample);
            return new Analysis(sample.GetState(), composition);
        }
        return null;
    }

    public void TransmitData(Controller controller, Data data)
    {
        if (state == "Работает")
        {
            Console.WriteLine("Геолог передал данные в Контроллер.");
        }
    }

    public string GetState()
    {
        return state;
    }
}
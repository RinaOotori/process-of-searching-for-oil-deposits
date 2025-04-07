namespace Testing_LR2.Objects;

public class SeismicSensor
{
    // Поля
    private string state;
    private List<SeismicData> recordedData;
    private DateTime lastCheck;

    // Конструктор
    public SeismicSensor()
    {
        state = "В режиме ожидания";
        recordedData = new List<SeismicData>();
        lastCheck = DateTime.Now;
    }

    // Методы
    public virtual void Activate()
    {
        state = "Активен";
        Console.WriteLine("Сейсмический датчик активирован.");
    }

    public void Deactivate()
    {
        state = "В режиме ожидания";
        Console.WriteLine("Сейсмический датчик деактивирован.");
    }

    public virtual SeismicData RecordSeismicData()
    {
        if (state == "Активен")
        {
            var data = new SeismicData(new Random().NextDouble() * 100);
            recordedData.Add(data);
            return data;
        }
        return null;
    }

    public virtual Data TransmitData(Controller controller)
    {
        var latestData = recordedData.LastOrDefault();
        if (latestData != null)
        {
            return new Data("SeismicSensor", latestData.Amplitude);
        }
        return null;
    }

    public virtual string CheckState()
    {
        return state;
    }

    public virtual bool SelfDiagnose()
    {
        lastCheck = DateTime.Now;
        return new Random().Next(0, 2) == 0; // Имитация диагностики
    }
}
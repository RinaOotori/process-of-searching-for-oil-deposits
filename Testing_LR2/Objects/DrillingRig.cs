namespace Testing_LR2.Objects;

public class DrillingRig
{
    // Поля
    private string state;
    private double drillingDepth;
    private List<OilSample> extractedSamples;

    // Конструктор
    public DrillingRig()
    {
        state = "Остановлена";
        drillingDepth = 0.0;
        extractedSamples = new List<OilSample>();
    }

    // Методы
    public virtual void StartDrilling()
    {
        state = "Работает";
        drillingDepth += new Random().Next(10, 100);
        Console.WriteLine($"Бурение начато. Текущая глубина: {drillingDepth} м.");
    }

    public void StopDrilling()
    {
        state = "Остановлена";
        Console.WriteLine("Бурение остановлено.");
    }

    public virtual OilSample ExtractSample()
    {
        if (state == "Работает")
        {
            var sample = new OilSample();
            extractedSamples.Add(sample);
            return sample;
        }
        return null;
    }

    public virtual string CheckState()
    {
        return state;
    }

    public virtual void PerformMaintenance()
    {
        if (state == "Требует ремонта")
        {
            state = "Остановлена";
            Console.WriteLine("Ремонт буровой установки завершен.");
        }
    }
}
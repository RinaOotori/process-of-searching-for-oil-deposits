namespace Testing_LR2.Objects;

public class OilSample
{
    // Поля
    private string state;
    private Dictionary<string, double> composition;
    private string sampleId;

    // Конструктор
    public OilSample()
    {
        state = "Не исследован";
        composition = new Dictionary<string, double>();
        sampleId = Guid.NewGuid().ToString();
    }

    // Методы
    public virtual void StartAnalysis()
    {
        state = "В процессе анализа";
        Console.WriteLine($"Анализ образца {sampleId} начат.");
    }

    public virtual void CompleteAnalysis(Dictionary<string, double> newComposition)
    {
        state = "Проанализирован";
        composition = newComposition;
        Console.WriteLine($"Анализ образца {sampleId} завершен.");
    }

    public virtual string GetState()
    {
        return state;
    }

    public Dictionary<string, double> GetComposition()
    {
        return state == "Проанализирован" ? composition : null;
    }
}
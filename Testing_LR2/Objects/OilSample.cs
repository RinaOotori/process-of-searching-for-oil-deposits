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
    public void StartAnalysis()
    {
        state = "В процессе анализа";
        Console.WriteLine($"Анализ образца {sampleId} начат.");
    }

    public void CompleteAnalysis(Dictionary<string, double> newComposition)
    {
        state = "Проанализирован";
        composition = newComposition;
        Console.WriteLine($"Анализ образца {sampleId} завершен.");
    }

    public string GetState()
    {
        return state;
    }

    public Dictionary<string, double> GetComposition()
    {
        return state == "Проанализирован" ? composition : null;
    }
}
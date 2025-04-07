namespace Testing_LR2.Objects;

public class Report
{
    // Поля
    private string state;
    private double reserveEstimate;
    private List<Analysis> analysisData;
    private DateTime completionDate;

    // Конструктор
    public Report()
    {
        state = "Не начат";
        reserveEstimate = 0.0;
        analysisData = new List<Analysis>();
        completionDate = DateTime.MinValue;
    }

    // Методы
    public void StartCompilation()
    {
        state = "В процессе составления";
        Console.WriteLine("Составление отчета начато.");
    }

    public void AddAnalysisData(Analysis analysis)
    {
        analysisData.Add(analysis);
    }

    public void SetReserveEstimate(double estimate)
    {
        reserveEstimate = estimate;
    }

    public void CompleteCompilation()
    {
        state = "Завершен";
        completionDate = DateTime.Now;
        Console.WriteLine("Отчет о запасах завершен.");
    }

    public string GetState()
    {
        return state;
    }

    public double GetReserveEstimate()
    {
        return state == "Завершен" ? reserveEstimate : 0.0;
    }
}
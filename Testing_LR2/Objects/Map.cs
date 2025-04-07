namespace Testing_LR2.Objects;

public class Map
{
    // Поля
    private string state;
    private Dictionary<string, object> terrainData;
    private DateTime creationDate;

    // Конструктор
    public Map()
    {
        state = "Не создана";
        terrainData = new Dictionary<string, object>();
        creationDate = DateTime.MinValue;
    }

    // Методы
    public void StartCreation()
    {
        state = "В процессе построения";
        Console.WriteLine("Построение карты начато.");
    }

    public void AddTerrainData(ProcessedData data)
    {
        terrainData["AverageAmplitude"] = data.ProcessedValues["AverageAmplitude"];
    }

    public void CompleteCreation()
    {
        state = "Завершена";
        creationDate = DateTime.Now;
        Console.WriteLine("Карта местности завершена.");
    }

    public string GetState()
    {
        return state;
    }

    public Dictionary<string, object> GetTerrainData()
    {
        return state == "Завершена" ? terrainData : null;
    }
}
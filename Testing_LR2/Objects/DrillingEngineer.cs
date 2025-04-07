namespace Testing_LR2.Objects;

public class DrillingEngineer
{
    // Поля
    private string state;
    private DrillingRig assignedRig;

    // Конструктор
    public DrillingEngineer(DrillingRig rig)
    {
        state = "Отдыхает";
        assignedRig = rig;
    }

    // Методы
    public void StartWork()
    {
        state = "Работает";
        Console.WriteLine("Буровой инженер начал работу.");
    }

    public void TakeBreak()
    {
        state = "Отдыхает";
        Console.WriteLine("Буровой инженер отдыхает.");
    }

    public void Leave()
    {
        state = "Отсутствует";
        Console.WriteLine("Буровой инженер отсутствует.");
    }

    public void StartDrilling(DrillingRig rig)
    {
        if (state == "Работает" && rig == assignedRig)
        {
            rig.StartDrilling();
        }
    }

    public void StopDrilling(DrillingRig rig)
    {
        if (state == "Работает" && rig == assignedRig)
        {
            rig.StopDrilling();
        }
    }

    public void ReceiveCommand(Controller controller, string command)
    {
        if (state == "Работает")
        {
            if (command == "Запустить бурение")
            {
                StartDrilling(assignedRig);
            }
            else if (command == "Остановить бурение")
            {
                StopDrilling(assignedRig);
            }
        }
    }

    public string GetState()
    {
        return state;
    }
}
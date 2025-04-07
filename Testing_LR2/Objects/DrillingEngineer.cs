namespace Testing_LR2.Objects;

public class DrillingEngineer
{
    // Поля
    public static string state;
    public static DrillingRig assignedRig;

    // Конструктор
    public DrillingEngineer(DrillingRig rig)
    {
        state = "Отдыхает";
        assignedRig = rig;
    }

    // Методы
    public virtual void StartWork()
    {
        state = "Работает";
        Console.WriteLine("Буровой инженер начал работу.");
    }

    public virtual void TakeBreak()
    {
        state = "Отдыхает";
        Console.WriteLine("Буровой инженер отдыхает.");
    }

    public virtual void Leave()
    {
        state = "Отсутствует";
        Console.WriteLine("Буровой инженер отсутствует.");
    }

    public virtual void StartDrilling(DrillingRig rig)
    {
        if (state == "Работает" && rig == assignedRig)
        {
            rig.StartDrilling();
        }
    }

    public virtual void StopDrilling(DrillingRig rig)
    {
        if (state == "Работает" && rig == assignedRig)
        {
            rig.StopDrilling();
        }
    }

    public virtual void ReceiveCommand(Controller controller, string command)
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

    public virtual string GetState()
    {
        return state;
    }
}
// Тип работ
public class WorkType
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Unit { get; set; }
    public int Price { get; set; }
    public int WorkKindID { get; set; }
    public WorkKind? WorkKind { get; set; }

    public List<BigRepairData> BigRepairData { get; set; } = new();

}

// Виды работ
public class WorkKind
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public List<WorkType> WorkTypes { get; set; } = new();
}

// Заказчики
public class Client
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public List<RepairObject> RepairObjects { get; set; } = new();
}

// Объекты заказчиков
public class RepairObject
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public Client? Client { get; set; }
    public List<BigRepairData> BigRepairData { get; set; } = new();
}

// Счета заказчиков - не реализовано!
public class Account
{
    public int Id { get; set; }
    public Client? Client { get; set; }
    public RepairObject? RepairObject { get; set; }
    public DateOnly? Date { get; set; }
    public double Amount { get; set; } 
}

// Исполнители работ
public class Master
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public List<BigRepairData> BigRepairData { get; set; } = new();
}

// Сметы на работы - не реализовано! 
public class WorkList
{
    public int Id { get; set; }
    public RepairObject? RepairObject { get; set; }
    public WorkType? WorkType { get; set; }
    public double Count { get; set; }
    public double Amount { get; set; }
}

// Выполненные работы
public class BigRepairData
{
    public int Id { get; set; }
    public RepairObject? RepairObject { get; set; }
    public Master? Master {  set; get; }
    public DateOnly? Date { get; set; }
    public WorkType? WorkType { get; set; }
    public double Count { get; set; }
    public double Amount { get; set;}
}


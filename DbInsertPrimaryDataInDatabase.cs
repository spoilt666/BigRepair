using OfficeOpenXml;
using System.Text;

public class InsertPrimaryDataInDB
{
    string fileNamePrimaryDataLoad;
    string fileNamePrimaryDataBigRepair;
    ApplicationContext db;

    public InsertPrimaryDataInDB(ApplicationContext db, string fileNamePrimaryDataLoad, string fileNamePrimaryDataBigRepair)
    {
        this.db = db;
        this.fileNamePrimaryDataLoad = fileNamePrimaryDataLoad;
        this.fileNamePrimaryDataBigRepair = fileNamePrimaryDataBigRepair;
    }
    public void InsertDataInDB()
    {
        if (db.WorkKinds.Count() == 0)
        {
            InsertDataWorkKinds();
        }

        if (db.WorkTypes.Count() == 0)
        {
            InsertDataWorkTypes();
        }
        if (db.Clients.Count() == 0)
        {
            InsertDataClientsAndRepairObjects();
        }
        if (db.Masters.Count() == 0)
        {
            InsertDataMasters();
        }
        if (db.BigRepairData.Count() ==0)
        {
            InsertDataBigRepairData();
        }
        
    }
    // Загрузка просто из списка готовыъ значений
    public void InsertDataWorkKinds()
    {
        
            string[] workKindsString = {"Организационные моменты",
            "Демонтажные работы",
            "Черновые и монтажные работы",
            "Инженерная сантехника",
            "Инженерная электрика",
            "Отделочные работы",
            "Чистовая сантехника",
            "Чистовая электрика"
            };
            for (int i = 0; i < workKindsString.Length; i++)
            {
                WorkKind kind = new WorkKind { Name = workKindsString[i] };
                db.WorkKinds.Add(kind);
            }
            db.SaveChanges();
        
    }
    // Загрузка из подготовленного текстового файла
    public void InsertDataWorkTypes()
    {

        if (!string.IsNullOrEmpty(fileNamePrimaryDataLoad))
        {
            string[] workTypeString = File.ReadAllLines(fileNamePrimaryDataLoad, Encoding.UTF8);
            for (int i = 0; i < workTypeString.Length; i++)
            {
                string[] workType = workTypeString[i].Split('\t');
                int workKindId = Convert.ToInt32(workType[0]);
                string workTypeName = workType[2];
                string workTypeUnit = workType[3];
                int workTypePrice = Convert.ToInt32(workType[4]);

                WorkKind workKind = db.WorkKinds.Where(w => w.Id == workKindId).FirstOrDefault();

                WorkType type = new WorkType { WorkKind = workKind, Name = workTypeName, Unit = workTypeUnit, Price = workTypePrice };
                db.WorkTypes.Add(type);
            }
            db.SaveChanges();
        }
    }

    // загрузка созданием объектов
    public void InsertDataClientsAndRepairObjects()
    {
        Client arc = new Client { Name = "Аркадий", PhoneNumber ="+79126574545", Email = "arc@mail.ru"};
        Client mic = new Client { Name = "Михаил", PhoneNumber = "+79128992113", Email = "mic@mail.ru" };
        db.Clients.AddRange(arc, mic);
        
        RepairObject arcRepairObject = new RepairObject { Client = arc, Name = "ЖК Облака", Address = "ЖК Облака" };
        RepairObject micRepairObject = new RepairObject { Client = mic, Name = "ЖК Новый город", Address = "ЖК Новый город" };
        db.RepairObjects.AddRange(arcRepairObject, micRepairObject);

        db.SaveChanges();
    }

    public void InsertDataMasters()
    {
        Master master = new Master { Name = "Роман Отделочник", PhoneNumber = "+79225038574", Email = "roman@mail.ru" };
        db.Masters.Add(master);
        db.SaveChanges();
    }

    // Загрузка из подготовленного эксель-файла
    public void InsertDataBigRepairData()
    {
        FileInfo fileInfo = new FileInfo(fileNamePrimaryDataBigRepair);
        var package = new ExcelPackage(fileInfo);
        // Лист с историческими данными
        var sheet = package.Workbook.Worksheets[0];

        for (int i = 2; i <= sheet.Dimension.End.Row; i++) 
        {
            BigRepairData bigRepairData = new BigRepairData();
            bigRepairData.Master = db.Masters.FirstOrDefault();
            bigRepairData.RepairObject = db.RepairObjects.FirstOrDefault();

            for (int j = 1; j <= 6; j++)
            {
                var value = sheet.Cells[i, j].Value;
                if (value != null)
                {
                    string stringValue = value.ToString();
                    switch(j)
                    {
                        case 1: //  Дата
                            DateOnly dateOnly = DateOnly.Parse(stringValue.Split(' ')[0]);
                            bigRepairData.Date = dateOnly;
                            break;
                        case 2: // Тип работ
                            string workTypeName = stringValue;
                            WorkType workType = db.WorkTypes.Where(t => t.Name == workTypeName).First();
                            if (workType != null)
                                bigRepairData.WorkType = workType;
                            break;
                        case 3:  // Количество 
                            bigRepairData.Count = Convert.ToDouble(stringValue);
                            break;
                        case 6: // Сумма
                            bigRepairData.Amount = Convert.ToDouble(stringValue);
                            break;

                    }
                    
                }
            }

            db.BigRepairData.Add(bigRepairData);
            db.SaveChanges();

        }
    }
}

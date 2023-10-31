using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory());
builder.AddJsonFile("appsettings.json");
var config = builder.Build();
// Подключение к БД
string connectionString = config.GetConnectionString("DefaultConnection");
// Файл с первичными данными по типам работ
string fileNamePrimaryDataLoad = config.GetConnectionString("PrimaryDataWorkType");
// Файл с первичными данными из общей таблицы
string fileNamePrimaryDataBigRepair = config.GetConnectionString("PrimaryDataBigRepair");

var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
var options = optionsBuilder.UseSqlite(connectionString).Options;

using (ApplicationContext db = new ApplicationContext(options))
{
    // Если база пустая, то заполнить историческими данными
    InsertPrimaryDataInDB insertPrimaryDataInDB = new InsertPrimaryDataInDB(db, fileNamePrimaryDataLoad, fileNamePrimaryDataBigRepair);
    insertPrimaryDataInDB.InsertDataInDB();

    ConsoleRender consoleRender = new ConsoleRender();
    // Выводим консольное меню и получаем параметры для отчета
    consoleRender.ShowMenu();
    int userChoice = consoleRender.GetUserChoice();
    DateOnly startDate = consoleRender.GetStartDate();
    DateOnly endDate = consoleRender.GetEndDate();
    
    if (userChoice > 0)
    {
        DbWorker dbWorker = new DbWorker(db);
        var bigRepairDataTable = dbWorker.GetBigRepairDataWithPeriod(startDate, endDate);

        Console.SetCursorPosition(0, 9);
        if (userChoice == 1)
            consoleRender.ShowBigRepairDataOnConsole(bigRepairDataTable);
        else
        {
            Console.WriteLine($"Генерация отчета по работам в excel-файл за период с {startDate} по {endDate}");
            ReportGenerator reportGenerator = new ReportGenerator();
            string fileName = $"D:\\Отчет {startDate} - {endDate}.xlsx";
            reportGenerator.GenerateReport(bigRepairDataTable, fileName, startDate, endDate);
            Console.WriteLine($"Файл сохранен: {fileName}");
        }
    }
    
    
}



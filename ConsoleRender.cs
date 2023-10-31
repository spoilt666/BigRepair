using System.Text;
public class ConsoleRender
{
    // Цвета для выбора в меню
    ConsoleColor activeColor = ConsoleColor.Green;
    ConsoleColor defaultColor = ConsoleColor.White;
    // Позиции курсора, определенные в программе 
    const int position1 = 3;
    const int position2 = 4;
    const int position3 = 5;
    // Буферный выбор
    int userChoice;
    public int UserChoice { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }

    public void ShowMenu()
    {
        bool isRunning = true;
        bool isMainMenu = true;
        int left = 25, top = 4;
        int prevLeft, prevTop;
        // Ограничение курсора для хождения по меню
        const int minTop = 3;
        const int maxTop = 5;
        // Ограничение для заполнения даты
        const int minLeft = 25;
        const int maxLeft = 35;
        // Позиция курсора для считывания даты
        const int firstPositionForStringDate = 25;
        // Доступные символы для ввода даты
        const string symbolsForDate = ".0123456789";
                
        // Шаблон для даты
        string stringStartDate = "06.07.2021"; // "__.__.____";
        string stringEndDate = "16.07.2021"; // "__.__.____";
        
        // Варианты работы программы
        // 1 - вывести данные по работам на консоль за период
        // 2 - сгенерировать отчет по работам за период
        // 3 - выход
        do
        {
            Console.CursorVisible = !isMainMenu;
            if (isMainMenu)
                ShowMainMenu(top);
            else
                ShowPeriodMenu(top, stringStartDate, stringEndDate);
            Console.SetCursorPosition(left, top);

            ConsoleKeyInfo charKey = Console.ReadKey();
            switch (charKey.Key)
            {
                case ConsoleKey.UpArrow:
                    if (top - 1 >= minTop)
                        top--;
                    left = firstPositionForStringDate;
                    break;
                case ConsoleKey.DownArrow:
                    if (top + 1 <= maxTop)
                        top++;
                    left = firstPositionForStringDate;
                    break;
                case ConsoleKey.Escape:
                    isRunning = false;
                    break;
                case ConsoleKey.Enter:
                    if (isMainMenu)
                    {
                        isMainMenu = false;
                        if (top == position1)
                            userChoice = 1;
                        else if (top == position2)
                            userChoice = 2;
                        else
                            isRunning = false;
                    }
                    else
                    {
                        if (top == position3)
                        {
                            isRunning = false;
                            StartDate = DateOnly.Parse(stringStartDate);
                            EndDate = DateOnly.Parse(stringEndDate);
                            UserChoice = userChoice;
                        }
                    }
                    break;
                default:
                    if ((symbolsForDate.IndexOf(charKey.KeyChar) > -1) 
                        || charKey.Key == ConsoleKey.LeftArrow
                        || charKey.Key == ConsoleKey.RightArrow)
                    {
                        if ((symbolsForDate.IndexOf(charKey.KeyChar) > -1))
                        {
                            var (x, y) = Console.GetCursorPosition();
                            if (top == position1)
                            {
                                StringBuilder sb = new StringBuilder(stringStartDate);
                                sb[x - firstPositionForStringDate - 1] = charKey.KeyChar;
                                stringStartDate = sb.ToString();
                            }
                            else if (top == position2)
                            {
                                StringBuilder sb = new StringBuilder(stringEndDate);
                                sb[x - firstPositionForStringDate - 1] = charKey.KeyChar;
                                stringEndDate = sb.ToString();
                            }
                            left = x;
                        }
                        else if (charKey.Key == ConsoleKey.LeftArrow)
                        {
                            if (left-1 >= minLeft)
                                left--;
                        }
                        else
                        {
                            if (left+1 < maxLeft)
                                left++;
                        }
                    }
                    break;
            }
        }
        while (isRunning);
    }

    void WriteWithColor(string str, bool isActive)
    {
        Console.ForegroundColor = isActive ? activeColor : defaultColor;
        Console.Write(str);
        Console.ForegroundColor = defaultColor;
    }

    void ShowMainMenu(int position)
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine();
        Console.WriteLine("******************** БОЛЬШОЙ РЕМОНТ *********************");
        Console.WriteLine("*                                                       *");
        Console.Write("*   ");
        WriteWithColor("1 - Вывести данные по работам на консоль за период  ", position == position1);
        Console.Write("*\n");
        Console.Write("*   ");
        WriteWithColor("2 - Сгенерировать отчет по работам за период        ", position == position2);
        Console.Write("*\n");
        Console.Write("*   ");
        WriteWithColor("3 - Выход из программы                              ", position == position3);
        Console.Write("*\n");
        Console.WriteLine("*                                                       *");
        Console.WriteLine("*********************************************************");
        Console.ResetColor();
    }

    void ShowPeriodMenu(int position, string stringStartDate, string stringEndDate)
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine();
        Console.WriteLine("************ БОЛЬШОЙ РЕМОНТ : ЗАПРОС ПЕРИОДА ************");
        Console.WriteLine("*                                                       *");
        Console.Write("*   ");
        WriteWithColor($"1 - Дата начала :    {stringStartDate}                     ", position == position1);
        Console.Write("*\n");
        Console.Write("*   ");
        WriteWithColor($"2 - Дата окончания : {stringEndDate}                     ", position == position2);
        Console.Write("*\n");
        Console.Write("*   ");
        WriteWithColor("3 - Запустить                                       ", position == position3);
        Console.Write("*\n");
        Console.WriteLine("*                                                       *");
        Console.WriteLine("*********************************************************");
        Console.ResetColor();
    }

    public int GetUserChoice()
    {
        return UserChoice;
    }

    public DateOnly GetStartDate()
    {
        return StartDate;
    }

    public DateOnly GetEndDate()
    {
        return EndDate;
    }

    public void ShowBigRepairDataOnConsole(List<BigRepairData> bigRepairDataTable)
    {
        Console.WriteLine($"Вывод данных по работам на консоль за период с {StartDate} по {EndDate}");
        double amount = 0;
        string header = String.Format("{0, -10} | {1, -50} | {2, -10} | {3, -10} | {4, -10}",
            "Дата", "Наименование работ", "Кол-во", "Ед. изм", "Цена", "Сумма"
            );
        Console.ForegroundColor = activeColor;
        Console.WriteLine(header);
        Console.ResetColor();

        foreach (BigRepairData bigRepairData in bigRepairDataTable)
        {
            string workTypeName = bigRepairData.WorkType?.Name;
            if (workTypeName.Length >= 50)
                workTypeName = workTypeName.Substring(0, 47);
            string str = String.Format("{0, -10} | {1, -50} | {2, -10} | {3, -10} | {4, -10}",
                bigRepairData.Date,
                workTypeName,
                $"{bigRepairData.Count} {bigRepairData.WorkType?.Unit}",
                $"{bigRepairData.WorkType?.Price} р.",
                $"= {bigRepairData.Amount} р."
            );
            Console.WriteLine(str);
            amount += bigRepairData.Amount;
        }
        Console.WriteLine();
        Console.ForegroundColor = activeColor;
        Console.WriteLine(String.Format("{0, 102}", $"ОБЩАЯ СТОИМОСТЬ РАБОТ | = {amount} р."));
        Console.ResetColor();
        Console.ReadKey();
    }
}


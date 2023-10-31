using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;

public class ReportGenerator
{
    public void GenerateReport(List<BigRepairData> bigRepairDataTable, string fileName, DateOnly startDate, DateOnly endDate)
    {
        // Объект ремонта
        string repairObjectName = bigRepairDataTable[0].RepairObject?.Name;
        
        var package = new ExcelPackage();
        
        DateTime now = DateTime.Today;
        var sheet = package.Workbook.Worksheets.Add(now.ToString("d"));

        // Шапка отчета
        sheet.Cells["A2:G2"].Merge = true;
        sheet.Cells["A2:G2"].Style.Font.Bold = true;
        sheet.Cells["A2:G2"].Style.Font.Size = 14;
        sheet.Cells["A2:G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        sheet.Cells["A2"].Value = $"Расчеты по объекту {repairObjectName}";

        sheet.Cells["A4:G4"].Merge = true;
        sheet.Cells["A4:G4"].Style.Font.Size = 12;
        sheet.Cells["A4:G4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        sheet.Cells["A4"].Value = $"Период {startDate} - {endDate}";
        
        // Шапка таблицы
        sheet.Cells["A6:G6"].Style.Font.Bold = true;
        sheet.Cells["A6:G6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        sheet.Cells["A6:G6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        sheet.Cells["A6:G6"].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
        sheet.Cells["A6:G6"].LoadFromArrays(new object[][] { new[] { "п/п", "Дата", "Наименование работ", "Кол-во", "Ед. изм", "Цена", "Сумма" } });

        sheet.Column(1).Width = 5;  // п/п
        sheet.Column(2).Width = 10; // Дата
        sheet.Column(3).Width = 60; // Наимеование работ
        sheet.Column(4).Width = 10; // Кол-во
        sheet.Column(5).Width = 10; // Ед. изм
        sheet.Column(6).Width = 10; // Цена
        sheet.Column(7).Width = 15; // Сумма
        sheet.Column(6).Style.Numberformat.Format = "# р.";
        sheet.Column(7).Style.Numberformat.Format = "# р.";
        int index = 1;
        int row = 7;
        double amount = 0;
        foreach (BigRepairData bigRepairData in bigRepairDataTable)
        {
            sheet.Cells[row, 1].Value = index;
            sheet.Cells[row, 2].Value = bigRepairData.Date;
            sheet.Cells[row, 3].Value = bigRepairData.WorkType.Name;
            sheet.Cells[row, 4].Value = bigRepairData.Count;
            sheet.Cells[row, 5].Value = bigRepairData.WorkType.Unit;
            sheet.Cells[row, 6].Value = bigRepairData.WorkType.Price;
            sheet.Cells[row, 7].Value = bigRepairData.Amount;
            index++;
            row++;
            amount += bigRepairData.Amount;
        }
        sheet.Cells[6, 1, row - 1, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        sheet.Cells[6, 1, row - 1, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        sheet.Cells[6, 1, row - 1, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        sheet.Cells[6, 1, row - 1, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        sheet.Cells[6, 1, row - 1, 7].Style.Border.BorderAround(ExcelBorderStyle.Thick);

        sheet.Cells[row + 1, 1, row + 1, 6].Merge = true;
        sheet.Cells[row + 1, 1, row + 1, 6].Style.Font.Bold = true;
        sheet.Cells[row + 1, 1, row + 1, 6].Style.Font.Size = 14;
        sheet.Cells[row + 1, 1, row + 1, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        sheet.Cells[row + 1, 1].Value = "ОБЩАЯ СТОИМОСТЬ РАБОТ";

        sheet.Cells[row + 1, 7].Style.Font.Bold = true;
        sheet.Cells[row + 1, 7].Style.Font.Size = 14;
        sheet.Cells[row + 1, 7].Value = amount;

        FileInfo fileInfo = new FileInfo(fileName);
        package.SaveAs(fileInfo);
    }
}


using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DbWorker
{
    ApplicationContext db;
    public DbWorker(ApplicationContext db) 
    { 
        this.db = db;
    }

    // Получмть данные за период
    public List<BigRepairData> GetBigRepairDataWithPeriod(DateOnly startDate, DateOnly endDate)
    {
        var bigRepairDataTable = db.BigRepairData.Where(b => b.Date >= startDate && b.Date <= endDate)
            .Include(b => b.WorkType)
            .Include(r => r.RepairObject)
            .ToList();
        return bigRepairDataTable;
    }
}


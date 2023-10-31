using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class ApplicationContext : DbContext
{
    public DbSet<WorkType> WorkTypes { get; set; } = null!;
    public DbSet<WorkKind> WorkKinds { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<RepairObject> RepairObjects { get; set; } = null!;
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Master> Masters { get; set; } = null!;
    public DbSet<WorkList> WorkLists { get; set; } = null!;
    public DbSet<BigRepairData> BigRepairData { get; set; } = null!;
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        //Database.EnsureCreated();
    }
    
}

public class SampleContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

        // получаем конфигурацию из файла appsettings.json
        ConfigurationBuilder builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        IConfigurationRoot config = builder.Build();

        // получаем строку подключения из файла appsettings.json
        string connectionString = config.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlite(connectionString);
        return new ApplicationContext(optionsBuilder.Options);
    }
}


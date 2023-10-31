using Hangfire;
using HangFireApp.BackGroundJobs;
using HangFireDbContextLibrary.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace HangFireApp
{
    class Program
    {
        static void Main()
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File(configuration["SeriLog:FilePath"], rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var dbContextOptions = new DbContextOptionsBuilder<HangFireDbContext>().UseSqlServer(configuration["ConnectionString:ConStr"]).Options;

            using (var context = new HangFireDbContext(dbContextOptions))
            {
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Log.Error("Veri tabanı güncellenemedi veya başarıyla oluşturulamadı. Mesaj: " + ex.Message);
                }
            }
            GlobalConfiguration.Configuration.UseSqlServerStorage(configuration["ConnectionString:ConStr"]);
            
            BackGroundJobRunGetStocksService jobRunGetStocksService = new BackGroundJobRunGetStocksService();

            using (var server = new BackgroundJobServer())
            {
                //5 dakikada bir çalıştırmak için Hangfire.
                RecurringJob.AddOrUpdate(() => jobRunGetStocksService.RunGetStocksService(configuration["Path:GetStockServicePath"]), "*/3 * * * *");

                Log.Information("Hangfire arka plan işlemleri başlatıldı. Çıkmak için ENTER'a basın.");
                Console.ReadLine();
            }
        }
    }
}


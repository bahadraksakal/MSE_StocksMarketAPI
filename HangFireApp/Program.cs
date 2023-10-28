using Hangfire;
using Microsoft.Extensions.Configuration;

namespace HangFireApp
{
    class Program
    {
        static void Main()
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            GlobalConfiguration.Configuration.UseSqlServerStorage(configuration["ConnectionString:ConStr"]);
            
            BackGroundJobRunGetStocksService jobRunGetStocksService = new BackGroundJobRunGetStocksService();

            using (var server = new BackgroundJobServer())
            {
                //5 dakikada bir çalıştırmak için Hangfire.
                RecurringJob.AddOrUpdate(() => jobRunGetStocksService.RunGetStocksService(configuration["Path:GetStockServicePath"]), "*/1 * * * *");

                Console.WriteLine("Hangfire arka plan işlemleri başlatıldı. Çıkmak için ENTER'a basın.");
                Console.ReadLine();
            }
        }
    }
}


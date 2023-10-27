using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Hangfire;

namespace HangFireApp
{
    class Program
    {
        static void Main()
        {

            string GetStocksServiceExePath = @"..\..\..\..\GetStocksService\\bin\\Debug\\net7.0\\GetStocksService.exe";

            GlobalConfiguration.Configuration.UseSqlServerStorage("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StockMarketDbHangFire;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            
            BackGroundJobRunGetStocksService jobRunGetStocksService = new BackGroundJobRunGetStocksService();
            
            using (var server = new BackgroundJobServer())
            {
                //5 dakikada bir çalıştırmak için Hangfire.
                RecurringJob.AddOrUpdate(() => jobRunGetStocksService.RunGetStocksService(GetStocksServiceExePath), "*/30 * * * * *");

                Console.WriteLine("Hangfire arka plan işlemleri başlatıldı. Çıkmak için ENTER'a basın.");
                Console.ReadLine();
            }
        }
    }
}


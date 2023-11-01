using Hangfire;
using HangFireApp.BackGroundJobs;
using HangFireDbContextLibrary.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using SharedServices.EmailServices;
using SharedServices.StockTrackingServices;
using StockMarketDbContextLibrary.Context;
using StocksMarketEntitiesLibrary.EmailEntities;
using System;

namespace HangFireApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            var serviceProvider = new ServiceCollection()
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<StockTrackingService>()
            .AddSingleton<BackGroundJobSendMailWhenPricesChanged>()
            .AddSingleton<BackGroundJobRunGetStocksService>()
            .AddDbContext<HangFireDbContext>(options => options.UseSqlServer(configuration["ConnectionString:ConStr"]))
            .AddDbContext<StockMarketDbContext>(options => options.UseSqlServer(configuration["ConnectionString:ConStrStockMarket"]))
            .BuildServiceProvider();

            var hangFireDbContext = serviceProvider.GetService<HangFireDbContext>();
            var stockMarketDbContext = serviceProvider.GetService<StockMarketDbContext>();
            try
            {

                hangFireDbContext.Database.Migrate();
                stockMarketDbContext.Database.Migrate();
            }
            catch(Exception ex)
            {
                Log.Error($"HangFireApp.Program:Main Veri tabanı başlangıçta oluşturulmadı: {ex.Message}");
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File(configuration["SeriLog:FilePath"], rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Emailer emailer = new Emailer
            {
                smtpServer = configuration["Emailer:SmtpServer"],
                smtpPort = int.Parse(configuration["Emailer:SmtpPort"]),
                username = configuration["Emailer:Username"],
                password = configuration["Emailer:Password"],
                from = configuration["Emailer:From"],
                enableSsl = true,
            };

            GlobalConfiguration.Configuration.UseSqlServerStorage(configuration["ConnectionString:ConStr"]);


            var options = new BackgroundJobServerOptions
            {
                Activator = new HangfireJobActivator(serviceProvider)
            };
            using (var server = new BackgroundJobServer(options))
            {
                //5 dakikada bir çalıştırmak için Hangfire.
                RecurringJob.AddOrUpdate<BackGroundJobRunGetStocksService>(
                    "job-run-get-stocks",
                    job => job.RunGetStocksService(configuration["Path:GetStockServicePath"]),
                    "*/3 * * * *");

                RecurringJob.AddOrUpdate<BackGroundJobSendMailWhenPricesChanged>(
                    "job-send-mail-prices-changed",
                    job => job.SendMailWhenPricesChanged(emailer),
                    "*/1 * * * *");

                Log.Information("Hangfire arka plan işlemleri başlatıldı. Çıkmak için ENTER'a basın.");
                Console.ReadLine();
            }
        }
    }
    public class HangfireJobActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public HangfireJobActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type jobType)
        {
            return _serviceProvider.GetRequiredService(jobType);
        }
    }

}


using HangFireDbContextLibrary.Context;
using Microsoft.Extensions.Configuration;
using Serilog;
using SharedServices.EmailServices;
using SharedServices.StockTrackingServices;
using StockMarketDbContextLibrary.Context;
using StockMarketEntitiesLibrary.Entities;
using StocksMarketEntitiesLibrary.EmailEntities;
using StocksMarketEntitiesLibrary.HangFireEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireApp.BackGroundJobs
{
    public class BackGroundJobSendMailWhenPricesChanged
    {
        private IEmailService _emailService;
        private StockTrackingService _stockTrackingService;
        private readonly HangFireDbContext _hangFireDbContext;
        private readonly StockMarketDbContext _stockMarketDbContext;
        public BackGroundJobSendMailWhenPricesChanged(HangFireDbContext hangFireDbContext,StockMarketDbContext stockMarketDbContext)
        {
            _hangFireDbContext = hangFireDbContext;
            _stockMarketDbContext = stockMarketDbContext;
            _emailService = new EmailService();
            _stockTrackingService = new StockTrackingService(_hangFireDbContext,_stockMarketDbContext);
        }
        public async Task SendMailWhenPricesChanged(Emailer emailer)
        {
            try
            {
                Dictionary<StockHangFire, List<User>> StockUserMap = await _stockTrackingService.ChangedPricesStocks();
                foreach (var keyValuePair in StockUserMap)
                {
                    emailer.toUserEmails = keyValuePair.Value.Select(user => user.Email).ToList();
                    emailer.subject = "Hisse Senedi Fiyat Değişikliği";
                    emailer.body = $"Değerli kullanıcımız: {keyValuePair.Key.Name} hisse senedinin fiyatı {keyValuePair.Key.Price} TL oldu";
                    await _emailService.SendEmailsAsync(emailer);
                }
                Log.Information("SendMailWhenPricesChanged tamamlandı");
            }
            catch (Exception ex)
            {
                Log.Error($"HangFireApp.BackGroundJobs:SendMailWhenPricesChanged: {ex.Message}");
            }
        }
    }
}

using Serilog;
using SharedServices.EmailServices;
using SharedServices.StockTrackingServices;
using StockMarketEntitiesLibrary.Entities;
using StocksMarketEntitiesLibrary.EmailEntities;
using StocksMarketEntitiesLibrary.HangFireEntities;

namespace HangFireApp.BackGroundJobs
{
    public class BackGroundJobSendMailWhenPricesChanged
    {
        private IEmailService _emailService;
        private StockTrackingService _stockTrackingService;
        public BackGroundJobSendMailWhenPricesChanged(IEmailService emailService, StockTrackingService stockTrackingService)
        {
            _emailService = emailService;
            _stockTrackingService = stockTrackingService;
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
                    try
                    {
                        await _emailService.SendEmailsAsync(emailer);
                    }
                    catch(Exception ex)
                    {
                        Log.Error($"HangFireApp.BackGroundJobs:SendMailWhenPricesChanged: Mesaj yollamaa işleminde hata: {ex.Message}");
                    }
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

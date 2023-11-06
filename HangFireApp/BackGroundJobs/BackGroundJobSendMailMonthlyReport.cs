using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedServices.EmailServices;
using SharedServices.ExcelServices;
using SharedServices.PdfServices;
using StockMarketDbContextLibrary.Context;
using StockMarketEntitiesLibrary.Entities;
using StocksMarketEntitiesLibrary.EmailEntities;

namespace HangFireApp.BackGroundJobs
{
    public class BackGroundJobSendMailMonthlyReport
    {
        private readonly IEmailService _emailService;
        private readonly ExportPdfService _exportPdfService;
        private readonly ExportExcelService _exportExcelService;
        private readonly StockMarketDbContext _stockMarketDbContext;
        public BackGroundJobSendMailMonthlyReport(StockMarketDbContext stockMarketDbContext, ExportPdfService exportPdfService, ExportExcelService exportExcelService, IEmailService emailService)
        {
            _stockMarketDbContext = stockMarketDbContext;
            _emailService = emailService;
            _exportPdfService = exportPdfService;
            _exportExcelService = exportExcelService;
        }
        public async Task SendMailMonthlyReport(Emailer emailer)
        {
            try
            {
                List<User> users = await _stockMarketDbContext.Users.AsNoTracking().ToListAsync();
                foreach (User user in users)
                {
                    try
                    {
                        string[] fileInfosPdf = await _exportPdfService.StockBuyAndSaleActivityByDateReportExportToPdf(user.Id, DateTime.Now.AddMonths(-1), DateTime.Now);
                        string[] fileInfosExcel = await _exportExcelService.StockBuyAndSaleActivityByDateReportExportToExcel(user.Id, DateTime.Now.AddMonths(-1), DateTime.Now);
                        string[] filePaths = { fileInfosPdf[0], fileInfosExcel[0] };
                        emailer.toUserEmail = user.Email;
                        emailer.subject = "CapitalWave Aylık Rapor Özeti";
                        emailer.body = $"Sayın {user.Name}, aylık raporunuz excel ve pdf formatında ekte yer almaktadır.";
                        await _emailService.SendEmailWithFilesAsync(emailer, filePaths);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"HangFireApp.BackGroundJobs:SendMailMonthlyReport: ForEach içinde hata: {ex.Message}");
                    }
                }
                Log.Information("BackGroundJobSendMailMonthlyReport tamamlandı");
            }
            catch (Exception ex)
            {
                Log.Error($"HangFireApp.BackGroundJobs:BackGroundJobSendMailMonthlyReport: Kullanıcıları çekerken hata - Db:  {ex.Message}");
            }
        
        }
    }
}

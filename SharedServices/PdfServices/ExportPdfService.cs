using iTextSharp.text;
using iTextSharp.text.pdf;
using StockMarketEntitiesLibrary.Entities;
using Serilog;
using StockMarketDbContextLibrary.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace SharedServices.PdfServices
{
    public class ExportPdfService
    {
        private readonly StockMarketDbContext _stockMarketDbContext;
        public ExportPdfService(StockMarketDbContext stockMarketDbContext)
        {
            _stockMarketDbContext = stockMarketDbContext;
        }
        public async Task<string[]> StockBuyAndSaleActivityByDateReportExportToPdf(int userId, DateTime startDate, DateTime endDate)
        {
            PortfolioUser portfolioUser = await _stockMarketDbContext.PortfolioUser
                                            .AsNoTracking()
                                            .Include(portfolioUser => portfolioUser.Portfolio)
                                            .Include(portfolioUser => portfolioUser.User)
                                            .Where(portfolioUser => portfolioUser.UserId == userId)
                                            .FirstOrDefaultAsync();
            if (portfolioUser == null)
            {
                Log.Error("ExportPdfService-StockBuyAndSaleActivityReportExportToPdf Hata: kullancıya ait portfolioUser bulunamadı");
                throw new Exception("ExportPdfService-StockBuyAndSaleActivityReportExportToPdf Hata: kullancıya ait portfolioUser bulunamadı");
            }
            List<StockBuyAndSale> userBuyAndSales = await _stockMarketDbContext.StockBuyAndSale
                                            .AsNoTracking()
                                            .Include(stockBuyAndSales => stockBuyAndSales.Stock)
                                            .Where(stockBuyAndSales => stockBuyAndSales.PortfolioId == portfolioUser.Portfolio.Id
                                            && stockBuyAndSales.Date >= startDate && stockBuyAndSales.Date <= endDate)
                                            .OrderByDescending(stockBuyAndSales => stockBuyAndSales.Stock.Name)
                                            .ToListAsync();
 #if DEBUG
            if (userBuyAndSales.IsNullOrEmpty())
            {
                
                Log.Warning("ExportPdfService-StockBuyAndSaleActivityReportExportToPdf: alınıp satılan hiç hisse yok.");
                throw new Exception("ExportPdfService-StockBuyAndSaleActivityReportExportToPdf: alınıp satılan hiç hisse yok.");
                
            }
#endif

            string fileName = $"{userId}-BuyAndSaleDetailInfoByDate";
            string directoryName = "BuyAndSaleDetailDateExportedPdf";
            string[] fileInfos = StockBuyAndSaleActivityToPdf(userId, userBuyAndSales, portfolioUser, fileName, directoryName);
            return fileInfos;
        }

        private void AddSummaryInfo(Document document, User user, Portfolio portfolio)
        {
            string calibriFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "calibri.ttf");
            BaseFont calibriFont = BaseFont.CreateFont(calibriFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font titleFont = new Font(calibriFont, 22f, Font.BOLD, BaseColor.Blue);
            Font normalFont = new Font(calibriFont, 14f, Font.NORMAL, BaseColor.Black);

            Paragraph title = new Paragraph("CapitalWave Şirketimiz Üzerinden Yapmış Olduğunuz İşlemlere İlişkin Özet Rapor", titleFont);
            title.Alignment = Element.ALIGN_CENTER;

            document.Add(title);
            document.Add(Chunk.Newline);
            document.Add(new Paragraph($"Sayın {user.Name}; güncel bakiyeniz ve işlem özetlerinize ilişkin mutabık değilseniz müşteri hizmetlerimizi arayınız. ", normalFont));
            document.Add(new Paragraph($"Güncel Bakiyeniz: {portfolio.Balance} TL", normalFont));
            document.Add(Chunk.Newline);
        }

        private string[] StockBuyAndSaleActivityToPdf(int userId, List<StockBuyAndSale> userBuyAndSales, PortfolioUser portfolioUser, string fileName, string directoryName)
        {

            var exportDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryName);
            string fileNameExtend = fileName + Guid.NewGuid().ToString() + ".pdf";
            string filePath = Path.Combine(exportDirectory, fileNameExtend);
            Directory.CreateDirectory(exportDirectory);

            using (var memoryStream = new MemoryStream()) // Değişiklik burada
            {
                using (Document document = new Document())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    AddSummaryInfo(document, portfolioUser.User, portfolioUser.Portfolio);

                    PdfPTable table = new PdfPTable(5); // Excel sütun sayısına göre ayarlayın

                    table.AddCell("StockName");
                    table.AddCell("Status");
                    table.AddCell("Unit");
                    table.AddCell("Price");
                    table.AddCell("Date");

                    foreach (var userBuyAndSale in userBuyAndSales)
                    {
                        table.AddCell(userBuyAndSale.Stock.Name);
                        table.AddCell(userBuyAndSale.Status);
                        table.AddCell(userBuyAndSale.Unit.ToString());
                        table.AddCell(userBuyAndSale.Price.ToString("#,##0.00"));
                        table.AddCell(userBuyAndSale.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    document.Add(table);
                }
                File.WriteAllBytes(filePath, memoryStream.ToArray());
            }

            Log.Information($"ExportPdfService-StockBuyAndSaleActivityToPdf: {fileName}.pdf saved to {directoryName} directory.");
            string[] fileInfos = new string[] { filePath, fileNameExtend };
            return fileInfos;
        }
    }
}

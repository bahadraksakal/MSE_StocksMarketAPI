using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Serilog;
using StockMarketDbContextLibrary.Context;
using StockMarketEntitiesLibrary.Entities;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace SharedServices.ExcelServices
{
    public class ExportExcelService
    {
        private readonly StockMarketDbContext _stockMarketDbContext;
        public ExportExcelService(StockMarketDbContext stockMarketDbContext)
        {
            _stockMarketDbContext = stockMarketDbContext;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public async Task<string[]> StockBuyAndSaleActivityByDateReportExportToExcel(int userId, DateTime startDate, DateTime endDate)
        {
            Portfolio portfolio = await _stockMarketDbContext.PortfolioUser
                                            .AsNoTracking()
                                            .Where(portfolioUser => portfolioUser.UserId == userId)
                                            .Select(portfolioUser => new Portfolio { Id = portfolioUser.PortfolioId })
                                            .FirstOrDefaultAsync();
            if (portfolio == null)
            {
                Log.Error("ExcelService-StockBuyAndSaleActivityReportExportToExcel Hata: kullancıya ait portfolio bulunamadı");
                throw new Exception("ExcelService-StockBuyAndSaleActivityReportExportToExcel Hata: kullancıya ait portfolio bulunamadı");
            }
            List<StockBuyAndSale> userBuyAndSales = await _stockMarketDbContext.StockBuyAndSale
                                            .AsNoTracking()
                                            .Include(stockBuyAndSales => stockBuyAndSales.Stock)
                                            .Where(stockBuyAndSales => stockBuyAndSales.PortfolioId == portfolio.Id
                                            && stockBuyAndSales.Date >= startDate && stockBuyAndSales.Date <= endDate)
                                            .OrderByDescending(stockBuyAndSales => stockBuyAndSales.Stock.Name)
                                            .ToListAsync();
            if (userBuyAndSales.IsNullOrEmpty())
            {
                Log.Warning("ExcelService-StockBuyAndSaleActivityReportExportToExcel: alınıp satılan hiç hisse yok.");
                throw new Exception("ExcelService-StockBuyAndSaleActivityReportExportToExcel: alınıp satılan hiç hisse yok.");
            }

            string worksheetName = "BuyAndSaleDetail - Date";
            string fileName = $"{userId}-BuyAndSaleDetailInfoByDate";
            string directoryName = "BuyAndSaleDetailDateExportedExcel";
            string[] fileInfos = StockBuyAndSaleActivityToExcel(userId, userBuyAndSales, worksheetName, fileName, directoryName);
            return fileInfos;
        }

        public async Task<string[]> StockBuyAndSaleActivityReportExportToExcel(int userId)
        {
            Portfolio portfolio = await _stockMarketDbContext.PortfolioUser
                                            .AsNoTracking()
                                            .Where(portfolioUser => portfolioUser.UserId == userId)
                                            .Select(portfolioUser => new Portfolio { Id = portfolioUser.PortfolioId })
                                            .FirstOrDefaultAsync();
            if (portfolio == null)
            {
                Log.Error("ExcelService-StockBuyAndSaleActivityReportExportToExcel Hata: kullancıya ait portfolio bulunamadı");
                throw new Exception("ExcelService-StockBuyAndSaleActivityReportExportToExcel Hata: kullancıya ait portfolio bulunamadı");
            }
            List<StockBuyAndSale> userBuyAndSales = await _stockMarketDbContext.StockBuyAndSale
                                            .AsNoTracking()
                                            .Include(stockBuyAndSales => stockBuyAndSales.Stock)
                                            .Where(stockBuyAndSales => stockBuyAndSales.PortfolioId == portfolio.Id)
                                            .OrderByDescending(stockBuyAndSales => stockBuyAndSales.Stock.Name)
                                            .ToListAsync();
            if (userBuyAndSales.IsNullOrEmpty())
            {
                Log.Warning("ExcelService-StockBuyAndSaleActivityReportExportToExcel: alınıp satılan hiç hisse yok.");
                throw new Exception("ExcelService-StockBuyAndSaleActivityReportExportToExcel: alınıp satılan hiç hisse yok.");
            }

            string worksheetName = "BuyandSaleDetailInfoAll";
            string fileName = $"{userId}-BuyAndSaleDetailInfo";
            string directoryName = "BuyAndSaleDetailExportedExcel";
            string[] fileInfos = StockBuyAndSaleActivityToExcel(userId, userBuyAndSales, worksheetName, fileName, directoryName);
            return fileInfos;
        }
        public async Task<string[]> StockPricesByNameExportToExcel(string stockName)
        {
            List<StockPrice> stockPrices = await _stockMarketDbContext.StockPrices
                                            .AsNoTracking()
                                            .Include(stockPrices => stockPrices.Stock)
                                            .Where(stockPrices => stockPrices.Stock.Status == false && stockPrices.Stock.Name == stockName)
                                            .Select(stockPrices => new StockPrice { Price = stockPrices.Price, Date = stockPrices.Date })
                                            .OrderByDescending(stockPrices => stockPrices.Date).ToListAsync();

            string worksheetName = stockName;
            string fileName = $"{stockName}StockPrices";
            string directoryName = "StocksPricesExportedExcel";

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(worksheetName);

                worksheet.Cells.AutoFitColumns(); // İlk iki sütunu otomatik boyutlandır
                worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Hücre içeriğini ortala
                worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center; // Hücre içeriğini ortala
                worksheet.Cells.Style.WrapText = false; // Uzun metinleri hücre içinde sığdır
                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 50;


                worksheet.Cells[1, 1].Value = "Price";
                worksheet.Cells[1, 1].AutoFilter = true;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                worksheet.Cells[1, 2].Value = "Date";
                worksheet.Cells[1, 2].AutoFilter = true;
                worksheet.Cells[1, 2].Style.Font.Bold = true;
                worksheet.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                int row = 2;
                foreach (var price in stockPrices)
                {
                    worksheet.Cells[row, 1].Value = price.Price;
                    worksheet.Cells[row, 1].Style.Numberformat.Format = "#,##0.00";

                    worksheet.Cells[row, 2].Value = price.Date;
                    worksheet.Cells[row, 2].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";

                    row++;
                }

                var exportDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryName);
                string fileNameExtend = fileName + Guid.NewGuid().ToString() + ".xlsx";
                var filePath = Path.Combine(exportDirectory, fileNameExtend);
                Directory.CreateDirectory(exportDirectory); // Klasörü oluştur, zaten varsa hiçbir şey yapma
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
                Log.Information($"ExportExcelService-ExportToExcel: {fileNameExtend} dosyası {exportDirectory} klasörüne kayıt edildi.");

                string[] fileInfos = new string[] { fileInfo.FullName, fileInfo.Name };
                return fileInfos;
            }

        }

        private string[] StockBuyAndSaleActivityToExcel(int userId, List<StockBuyAndSale> userBuyAndSales, string worksheetName, string fileName, string directoryName)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(worksheetName);

                worksheet.Cells.AutoFitColumns(); // İlk iki sütunu otomatik boyutlandır
                worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Hücre içeriğini ortala
                worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center; // Hücre içeriğini ortala
                worksheet.Cells.Style.WrapText = false; // Uzun metinleri hücre içinde sığdır
                worksheet.Column(1).Width = 20;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 20;
                worksheet.Column(4).Width = 20;
                worksheet.Column(5).Width = 50;


                worksheet.Cells[1, 1].Value = "StockName";
                worksheet.Cells[1, 1].AutoFilter = true;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                worksheet.Cells[1, 2].Value = "Status";
                worksheet.Cells[1, 2].AutoFilter = true;
                worksheet.Cells[1, 2].Style.Font.Bold = true;
                worksheet.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                worksheet.Cells[1, 3].Value = "Unit";
                worksheet.Cells[1, 3].AutoFilter = true;
                worksheet.Cells[1, 3].Style.Font.Bold = true;
                worksheet.Cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                worksheet.Cells[1, 4].Value = "Price";
                worksheet.Cells[1, 4].AutoFilter = true;
                worksheet.Cells[1, 4].Style.Font.Bold = true;
                worksheet.Cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                worksheet.Cells[1, 5].Value = "Date";
                worksheet.Cells[1, 5].AutoFilter = true;
                worksheet.Cells[1, 5].Style.Font.Bold = true;
                worksheet.Cells[1, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                int row = 2;
                foreach (var userBuyAndSale in userBuyAndSales)
                {
                    worksheet.Cells[row, 1].Value = userBuyAndSale.Stock.Name;

                    worksheet.Cells[row, 2].Value = userBuyAndSale.Status;

                    worksheet.Cells[row, 3].Value = userBuyAndSale.Unit;

                    worksheet.Cells[row, 4].Value = userBuyAndSale.Price;
                    worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";

                    worksheet.Cells[row, 5].Value = userBuyAndSale.Date;
                    worksheet.Cells[row, 5].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";

                    row++;
                }

                var exportDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryName);
                string fileNameExtend = fileName + Guid.NewGuid().ToString() + ".xlsx";
                var filePath = Path.Combine(exportDirectory, fileNameExtend);
                Directory.CreateDirectory(exportDirectory); // Klasörü oluştur, zaten varsa hiçbir şey yapma
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
                Log.Information($"ExportExcelService-ExportToExcel: {fileNameExtend} dosyası {exportDirectory} klasörüne kayıt edildi.");

                string[] fileInfos = new string[] { fileInfo.FullName, fileInfo.Name };
                return fileInfos;
            }
        }
    }
}

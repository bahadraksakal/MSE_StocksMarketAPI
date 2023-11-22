using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml;
using Serilog;
using SharedServices.ExcelServices;
using StockMarketDbContextLibrary.Context;
using StockMarketEntitiesLibrary.Entities;
using Xunit;

//default olarak paralel çalışır. true yaparsak parallelik kapanır.


[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Tests
{
    
    public class ExportExcelServiceTests : IDisposable
    {
        private readonly StockMarketDbContext _dbContext;
        private readonly SqliteConnection _connection;
        private IDbContextTransaction _transaction;

        public ExportExcelServiceTests()
        {
            SQLitePCL.Batteries.Init();

            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<StockMarketDbContext>()
                .UseSqlite(_connection)
                .Options;

            _dbContext = new StockMarketDbContext(options);

            //transection önce yap
            _dbContext.Database.EnsureDeleted(); // Varolan veritabanını sil
            _dbContext.Database.EnsureCreated();

            _transaction = _dbContext.Database.BeginTransaction();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            SeedTestData(); // Yalnızca bir kere seed işlemi gerçekleştirilecek
        }

        public void Dispose()
        {            
            _transaction.Dispose();
            _dbContext.Dispose();
            _connection.Close();
        }

        [Fact]
        public async Task StockBuyAndSaleActivityByDateReportExportToExcel_ShouldExportFile()
        {
            
            var service = new ExportExcelService(_dbContext);
            var userId = 1;
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 1, 2);

            
            var result = await service.StockBuyAndSaleActivityByDateReportExportToExcel(userId, startDate, endDate);

            FileInfo fileInfo = new FileInfo(result[0]);

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // İlk çalışma sayfasını al              

                Assert.Equal("TestStock",worksheet.Cells[2, 1].Value?.ToString());
                Assert.Equal("Buy", worksheet.Cells[2, 2].Value?.ToString());
                Assert.Equal("10", worksheet.Cells[2, 3].Value?.ToString());
                Assert.Equal("100", worksheet.Cells[2, 4].Value?.ToString());
                Assert.Equal(new DateTime(2023, 1, 1), worksheet.Cells[2, 5].GetValue<DateTime?>());

                Assert.Equal("TestStock", worksheet.Cells[3, 1].Value?.ToString());
                Assert.Equal("Buy", worksheet.Cells[3, 2].Value?.ToString());
                Assert.Equal("10", worksheet.Cells[3, 3].Value?.ToString());
                Assert.Equal("100", worksheet.Cells[3, 4].Value?.ToString());
                Assert.Equal(new DateTime(2023, 1, 1), worksheet.Cells[3, 5].GetValue<DateTime?>());

                Assert.Equal("TestStock", worksheet.Cells[4, 1].Value?.ToString());
                Assert.Equal("Sell", worksheet.Cells[4, 2].Value?.ToString());
                Assert.Equal("10", worksheet.Cells[4, 3].Value?.ToString());
                Assert.Equal("200", worksheet.Cells[4, 4].Value?.ToString());
                Assert.Equal(new DateTime(2023, 1, 2), worksheet.Cells[4, 5].GetValue<DateTime?>());
            }

            Assert.NotNull(result);
            Assert.Equal(2, result.Length); // Dosya adı ve dosya yolu bekleniyor
            Assert.True(File.Exists(result[0])); // Dosyanın varlığını kontrol et

            _transaction.Rollback();
        }

        [Fact]
        public async Task StockBuyAndSaleActivityReportExportToExcel_ShouldExportFile()
        {
            // Arrange
            var service = new ExportExcelService(_dbContext);
            var userId = 1;

            // Act
            var result = await service.StockBuyAndSaleActivityReportExportToExcel(userId);

            FileInfo fileInfo = new FileInfo(result[0]);

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // İlk çalışma sayfasını al              

                Assert.Equal("TestStock", worksheet.Cells[2, 1].Value?.ToString());
                Assert.Equal("Buy", worksheet.Cells[2, 2].Value?.ToString());
                Assert.Equal("10", worksheet.Cells[2, 3].Value?.ToString());
                Assert.Equal("100", worksheet.Cells[2, 4].Value?.ToString());
                Assert.Equal(new DateTime(2023, 1, 1), worksheet.Cells[2, 5].GetValue<DateTime?>());

                Assert.Equal("TestStock", worksheet.Cells[3, 1].Value?.ToString());
                Assert.Equal("Buy", worksheet.Cells[3, 2].Value?.ToString());
                Assert.Equal("10", worksheet.Cells[3, 3].Value?.ToString());
                Assert.Equal("100", worksheet.Cells[3, 4].Value?.ToString());
                Assert.Equal(new DateTime(2023, 1, 1), worksheet.Cells[3, 5].GetValue<DateTime?>());

                Assert.Equal("TestStock", worksheet.Cells[4, 1].Value?.ToString());
                Assert.Equal("Sell", worksheet.Cells[4, 2].Value?.ToString());
                Assert.Equal("10", worksheet.Cells[4, 3].Value?.ToString());
                Assert.Equal("200", worksheet.Cells[4, 4].Value?.ToString());
                Assert.Equal(new DateTime(2023, 1, 2), worksheet.Cells[4, 5].GetValue<DateTime?>());

                Assert.Equal("TestStock", worksheet.Cells[5, 1].Value?.ToString());
                Assert.Equal("Buy", worksheet.Cells[5, 2].Value?.ToString());
                Assert.Equal("30", worksheet.Cells[5, 3].Value?.ToString());
                Assert.Equal("50", worksheet.Cells[5, 4].Value?.ToString());
                Assert.Equal(new DateTime(2023, 3, 4), worksheet.Cells[5, 5].GetValue<DateTime?>());

                Assert.Equal("TestStock", worksheet.Cells[6, 1].Value?.ToString());
                Assert.Equal("Sell", worksheet.Cells[6, 2].Value?.ToString());
                Assert.Equal("30", worksheet.Cells[6, 3].Value?.ToString());
                Assert.Equal("50", worksheet.Cells[6, 4].Value?.ToString());
                Assert.Equal(new DateTime(2023, 3, 5), worksheet.Cells[6, 5].GetValue<DateTime?>());
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length); // Dosya adı ve dosya yolu bekleniyor
            Assert.True(File.Exists(result[0])); // Dosyanın varlığını kontrol et

            _transaction.Rollback();
        }

        [Fact]
        public async Task StockPricesByNameExportToExcel_ShouldExportFile()
        {
            // Arrange
            var service = new ExportExcelService(_dbContext);
            var stockName = "TestStock";

            // Act
            var result = await service.StockPricesByNameExportToExcel(stockName);

            FileInfo fileInfo = new FileInfo(result[0]);

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // İlk çalışma sayfasını al              

                Assert.Equal("TestStock", worksheet.Name); 

                //order by date e göre 
                Assert.Equal("200", worksheet.Cells[2, 1].Value?.ToString());
                Assert.Equal(new DateTime(2023, 1, 2), worksheet.Cells[2, 2].GetValue<DateTime?>());
                
                Assert.Equal("100", worksheet.Cells[3, 1].Value?.ToString());
                Assert.Equal(new DateTime(2023, 1, 1), worksheet.Cells[3, 2].GetValue<DateTime?>());
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length); // Dosya adı ve dosya yolu bekleniyor
            Assert.True(File.Exists(result[0])); // Dosyanın varlığını kontrol et

            _transaction.Rollback();
        }

        private void SeedTestData()
        {
            if (!_dbContext.Stocks.Any())
            {
                _dbContext.Stocks.Add(new Stock { Name = "TestStock", Unit = 9990, Status = false });
                _dbContext.SaveChanges();
                _dbContext.StockPrices.Add(new StockPrice { StockId = 1, Price = 100, Date = new DateTime(2023, 1, 1) });
                _dbContext.SaveChanges();
                _dbContext.StockPrices.Add(new StockPrice { StockId = 1, Price = 200, Date = new DateTime(2023, 1, 2) });


                _dbContext.StockBuyAndSale.Add(new StockBuyAndSale { StockId = 1, PortfolioId = 1, Price = 100, Date = new DateTime(2023, 1, 1), Unit = 10, Status = "Buy" });
                _dbContext.StockBuyAndSale.Add(new StockBuyAndSale { StockId = 1, PortfolioId = 1, Price = 100, Date = new DateTime(2023, 1, 1), Unit = 10, Status = "Buy" });
                _dbContext.StockBuyAndSale.Add(new StockBuyAndSale { StockId = 1, PortfolioId = 1, Price = 200, Date = new DateTime(2023, 1, 2), Unit = 10, Status = "Sell" });
                _dbContext.SaveChanges();
                _dbContext.StockBuyAndSale.Add(new StockBuyAndSale { StockId = 1, PortfolioId = 1, Price = 50, Date = new DateTime(2023, 3, 4), Unit = 30, Status = "Buy" });
                _dbContext.StockBuyAndSale.Add(new StockBuyAndSale { StockId = 1, PortfolioId = 1, Price = 50, Date = new DateTime(2023, 3, 5), Unit = 30, Status = "Sell" });
                _dbContext.SaveChanges();
                _dbContext.PortfolioStock.Add(new PortfolioStock { PortfolioId = 1, StockId = 1, Unit = 10, IsTracked = true });
                _dbContext.SaveChanges();
            }

        }

    }

}

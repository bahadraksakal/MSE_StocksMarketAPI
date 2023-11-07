using HangFireDbContextLibrary.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StockMarketDbContextLibrary.Context;
using StockMarketEntitiesLibrary.Entities;
using StocksMarketEntitiesLibrary.HangFireEntities;

namespace SharedServices.StockTrackingServices
{
    public class StockTrackingService
    {
        private readonly HangFireDbContext _hangFireDbContext;
        private readonly StockMarketDbContext _stockMarketDbContext;
        public StockTrackingService(HangFireDbContext hangFireDbContext, StockMarketDbContext stockMarketDbContext)
        {
            _hangFireDbContext = hangFireDbContext;
            _stockMarketDbContext = stockMarketDbContext;
        }

        public async Task<Dictionary<StockHangFire, List<User>>> ChangedPricesStocks()
        {
            // veritabanı işlemleri
            List<StockPrice> stockPrices = await _stockMarketDbContext.StockPrices
                                            .AsNoTracking()
                                            .Include(stockPrices => stockPrices.Stock)
                                            .Where(stockPrice => stockPrice.Stock.Status == false)
                                            .GroupBy(stockPrice => stockPrice.StockId)
                                            .Select(group => group.OrderByDescending(stockPrice => stockPrice.Date).First())
                                            .ToListAsync();
            if (stockPrices.IsNullOrEmpty())
            {
                Log.Warning("stockTrackingService-ChangedPricesStocks: StockPrices boş lütfen önce bigpara API den stock verilerini çekin");
                throw new Exception("stockTrackingService-ChangedPricesStocks: StockPrices boş lütfen önce bigpara API den stock verilerini çekin");
            }

            List<StockHangFire> stocksHangFire = await _hangFireDbContext.StockHangFire.ToListAsync();

            //fiyatı değişen hisse sentlerinin tespiti
            List<StockHangFire> stocksHangFireChanged = new List<StockHangFire>();
            foreach(StockPrice stockPrice in stockPrices)
            {
                StockHangFire stockHangFire = stocksHangFire.FirstOrDefault(stockHangFire => stockHangFire.Name == stockPrice.Stock.Name);
                //Log.Warning($"-StockTrackingService-ChangedPricesStocks: {stockPrice.Stock.Name} için çalışıyor.");
                if (stockHangFire == null)
                {
                    stockHangFire = new StockHangFire
                    {
                        Name = stockPrice.Stock.Name,
                        Price = stockPrice.Price
                    };
                    await _hangFireDbContext.StockHangFire.AddAsync(stockHangFire);
                    Log.Information($"StockTrackingService-ChangedPricesStocks: {stockHangFire.Name} eklendi");
                    continue;
                }
                if (stockHangFire.Price == stockPrice.Price)
                {
                    continue;
                }
                if (stockHangFire.Price != stockPrice.Price)
                {
                    stockHangFire.Price = stockPrice.Price;
                    Log.Information($"{stockHangFire.Name} Fiyat değişti");
                    stocksHangFireChanged.Add(stockHangFire);
                }
            }
            await _hangFireDbContext.SaveChangesAsync();
            //Log.Warning("Değişikler başarıyla kayıt edildi.");
            //fiyatı değişen hisse senetlerine sahip kullanıcıların tespiti.
            Dictionary<StockHangFire, List<User>> userChangedStockMap = new Dictionary<StockHangFire, List<User>>();
            foreach(StockHangFire stockHangFire in stocksHangFireChanged)
            {
                List<User> users = await _stockMarketDbContext.PortfolioStock
                                                .AsNoTracking()
                                                .Include(portfolioStocks => portfolioStocks.Stock)
                                                .Include(portfolioStocks => portfolioStocks.Portfolio)
                                                .ThenInclude(portfolio => portfolio.PortfolioUsers)
                                                .ThenInclude(portfolioUser => portfolioUser.User)
                                                .Where(portfolioStocks => portfolioStocks.Stock.Name == stockHangFire.Name
                                                && portfolioStocks.Stock.Status == false 
                                                && portfolioStocks.Unit>0
                                                && portfolioStocks.IsTracked == true)
                                                .Select(portfolioStocks => new User
                                                {
                                                    Id = portfolioStocks.Portfolio.PortfolioUsers.User.Id,
                                                    Name = portfolioStocks.Portfolio.PortfolioUsers.User.Name,
                                                    Email = portfolioStocks.Portfolio.PortfolioUsers.User.Email
                                                })
                                                .ToListAsync();
                //List<User> users = _stockMarketDbContext.PortfolioUser
                //                .AsNoTracking()
                //                .Include(portfolioUser => portfolioUser.User)
                //                .Where(portfolioUser => portfolioStocks.Contains(portfolioUser.PortfolioId))
                //                .Select(portfolio => portfolio.User)
                //                .ToList();
                if(users.IsNullOrEmpty())
                {
                    //Log.Warning("users null or empty");
                    continue;
                } 
                userChangedStockMap.Add(stockHangFire, users);
            }

            return userChangedStockMap;
        }
    }
}

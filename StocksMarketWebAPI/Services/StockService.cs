using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.DTOs.PortfolioStockDTOs;
using StocksMarketWebAPI.DTOs.StockPriceDTOs;
using StocksMarketWebAPI.Entities;

namespace StocksMarketWebAPI.Services
{
    public class StockService
    {
        private readonly StockMarketDbContext _stockMarketDbContext;
        private readonly IMapper _mapper;
        
        public StockService(StockMarketDbContext stockMarketDbContext, IMapper mapper)
        {
            _stockMarketDbContext = stockMarketDbContext;
            _mapper = mapper;   
        }

        public async Task<string> SellStockAsync(int userId, int sellingStockUnit, string stockName)
        {
            PortfolioUser portfolioUser = await _stockMarketDbContext.PortfolioUser
                .Include(portfolioUser => portfolioUser.User)
                .Include(portfolioUser => portfolioUser.Portfolio)
                .FirstOrDefaultAsync(portfolioUser => portfolioUser.UserId == userId);
            if (portfolioUser == null)
            {
                Log.Warning($"StockService-BuyStockAsync Hata: {userId} id li bir kullanıcı yok.");
                throw new Exception($"StockService-BuyStockAsync Hata: {userId} id li bir kullanıcı yok.");
            }
            //order by kullanırken first or default içinde where tarzı bir yazım yapmak ef tarafından önerilmemektedir.
            StockPrice stockPrice = await _stockMarketDbContext.StockPrices
                .Include(stockPrices => stockPrices.Stock)
                .Where(stockPrice => stockPrice.Stock.Name == stockName && stockPrice.Stock.Status == false)
                .OrderByDescending(stockPrice => stockPrice.Date)
                .FirstOrDefaultAsync();
            if (stockPrice == null)
            {
                Log.Warning($"StockService-BuyStockAsync Hata: stockPrice bulunamadı." +
                   $" {stockName} isimli hisse senedine ait veri olamayabilir veya hisse senedi işleme kapalı olabilir");
                throw new Exception($"StockService-BuyStockAsync Hata: portfolioStock bulunamadı." +
                    $" {stockName} isimli hisse senedine ait veri olamayabilir veya hisse senedi işleme kapalı olabilir");
            }

            MainBoard mainBoard = await _stockMarketDbContext.MainBoards.FirstOrDefaultAsync();
            if (mainBoard == null)
            {
                Log.Warning($"StockService-BuyStockAsync Hata: MainBoard bulunamadı");
                throw new Exception("StockService-BuyStockAsync Hata: MainBoard bulunamadı");
            }

            // bu sefer burası null olamaz. Satış işlemi için önceden hisse kesinlikle alınmış olmalıdır.
            PortfolioStock portfolioStock = await _stockMarketDbContext.PortfolioStock
                .Include(portfolioStock => portfolioStock.Stock)
                .FirstOrDefaultAsync(portfolioStock => portfolioStock.Stock.Name == stockName
                && portfolioStock.PortfolioId == portfolioUser.PortfolioId
                && portfolioStock.Stock.Status == false);
            if(portfolioStock == null)
            {
                Log.Warning($"StockService-BuyStockAsync Hata: Bu hisseden elinizde hiç bulunmuyor veya hisse işleme kapalı.");
                throw new Exception("StockService-BuyStockAsync Hata: Bu hisseden elinizde hiç bulunmuyor veya hisse işleme kapalı.");
            }
            //Kurallar - portfoyümdeki hisse adeti yeterli mi - komisyondan sonra kalan para yeterli mi (portfoy bakiyesi+ satılan hisse değeri)
            int stockLeft = portfolioStock.Unit - sellingStockUnit;
            if (stockLeft < 0)
            {
                Log.Warning($"StockService-BuyStockAsync Hata: Satmak istediğiniz kadar hisse senedine sahip değilsiniz." +
                    $" Portföyünüzdeki hisse senedi adeti: {portfolioStock.Unit}" +
                    $" Satmak istediğini miktar: {sellingStockUnit}");
                throw new Exception($"StockService-BuyStockAsync Hata: Satmak istediğiniz kadar hisse senedine sahip değilsiniz." +
                    $" Portföyünüzdeki hisse senedi adeti: {portfolioStock.Unit}" +
                    $" Satmak istediğini miktar: {sellingStockUnit}");
            }

            float transactionMoney = stockPrice.Price * sellingStockUnit;
            float commissionAmount = transactionMoney * ((float)mainBoard.CommissionRate / 100);
            float newPortfolioBalance = portfolioUser.Portfolio.Balance + transactionMoney - commissionAmount;
            if (newPortfolioBalance < 0)
            {
                Log.Warning($" StockService-BuyStockAsync Hata:" +
                    $" {transactionMoney} TL'lik işlem miktarı için ödemeniz gerek komisyon {commissionAmount} TL'dir." +
                    $" İşlem sonrası yeni bakiyeniz {newPortfolioBalance} TL'dir. Bakiye 0'ın altına düşemez!");
                throw new Exception($" StockService-BuyStockAsync Hata:" +
                    $" {transactionMoney} TL'lik işlem miktarı için ödemeniz gerek komisyon {commissionAmount} TL'dir." +
                    $" İşlem sonrası yeni bakiyeniz {newPortfolioBalance} TL'dir. Bakiye 0'ın altına düşemez!");
            }

            mainBoard.Balance += commissionAmount; // komisyonu ekle
            float oldPortfolioBalance = portfolioUser.Portfolio.Balance; // eski bakiyesini tut.
           
            portfolioUser.Portfolio.Balance = portfolioUser.Portfolio.Balance + transactionMoney - commissionAmount; // satış parasını ekle. komisyonu al.
            portfolioStock.Unit -= sellingStockUnit;  // portföyümseki ilgili hisse adeti arttırıldı.
            stockPrice.Stock.Unit = stockPrice.Stock.Unit + sellingStockUnit; // sistemdeki hisse senedi adeti eksiltildi.

            //veri tabanına detaylı kaydı ekleme.
            await _stockMarketDbContext.StockBuyAndSale.AddAsync(new StockBuyAndSale
            {
                PortfolioId = portfolioUser.PortfolioId,
                StockId = stockPrice.Stock.Id,
                Unit = sellingStockUnit,
                Status = "Sell", //alım demek
                Price = stockPrice.Price, // kaç tl den aldım.
                Date = DateTime.Now
            });

            await _stockMarketDbContext.SaveChangesAsync();

            Log.Warning($"{portfolioStock.Stock.Name} kodlu hisse senedinden," +
                $" {stockPrice.Price} TL fiyatından, {sellingStockUnit} adet sattınız." +
                $" İşlem {transactionMoney} TL, komisyon {commissionAmount} TL tuttu." +
                $" Eski bakiyeniz {oldPortfolioBalance} TL.\nYeni bakiyeniz {portfolioUser.Portfolio.Balance} TL." +
                $" Sistemde kalan hisse senedi {portfolioStock.Stock.Unit} adettir.");
            return $"{portfolioStock.Stock.Name} kodlu hisse senedinden," +
                $" {stockPrice.Price} TL fiyatından, {sellingStockUnit} adet sattınız." +
                $" İşlem {transactionMoney} TL, komisyon {commissionAmount} TL tuttu." +
                $" Eski bakiyeniz {oldPortfolioBalance} TL.\nYeni bakiyeniz {portfolioUser.Portfolio.Balance} TL." +
                $" Sistemde kalan hisse senedi {portfolioStock.Stock.Unit} adettir.";

        }

        public async Task<string> BuyStockAsync(int userId, int buyingStockUnit, string stockName)
        {
            PortfolioUser portfolioUser = await _stockMarketDbContext.PortfolioUser
                .Include(portfolioUser => portfolioUser.User)
                .Include(portfolioUser => portfolioUser.Portfolio)
                .FirstOrDefaultAsync(portfolioUser => portfolioUser.UserId == userId);
            if(portfolioUser == null)
            {
                Log.Warning($"StockService-BuyStockAsync Hata: {userId} id li bir kullanıcı yok.");
                throw new Exception($"StockService-BuyStockAsync Hata: {userId} id li bir kullanıcı yok.");
            }

            //order by kullanırken first or default içinde where tarzı bir yazım yapmak ef tarafından önerilmemektedir.
            StockPrice stockPrice = await _stockMarketDbContext.StockPrices
                .Include(stockPrices => stockPrices.Stock)
                .Where(stockPrice => stockPrice.Stock.Name == stockName && stockPrice.Stock.Status == false)
                .OrderByDescending(stockPrice => stockPrice.Date)
                .FirstOrDefaultAsync();

            if (stockPrice == null)
            {
                Log.Warning($"StockService-BuyStockAsync Hata: stockPrice bulunamadı." +
                   $" {stockName} isimli hisse senedine ait veri olamayabilir veya hisse senedi işleme kapalı olabilir");
                throw new Exception($"StockService-BuyStockAsync Hata: portfolioStock bulunamadı." +
                    $" {stockName} isimli hisse senedine ait veri olamayabilir veya hisse senedi işleme kapalı olabilir");
            }

            MainBoard mainBoard = await _stockMarketDbContext.MainBoards.FirstOrDefaultAsync();
            if(mainBoard == null)
            {
                Log.Warning($"StockService-BuyStockAsync Hata: MainBoard bulunamadı");
                throw new Exception("StockService-BuyStockAsync Hata: MainBoard bulunamadı");
            }

            // bu değer null olabilir, bir kullanıcı daha  önce o hisseden hiç almamış olabilir.
            PortfolioStock portfolioStock = await _stockMarketDbContext.PortfolioStock
                .Include(portfolioStock => portfolioStock.Stock)
                .FirstOrDefaultAsync(portfolioStock => portfolioStock.Stock.Name == stockName
                && portfolioStock.PortfolioId == portfolioUser.PortfolioId
                && portfolioStock.Stock.Status == false);

            //Kurallar - hisse adeti yeterli mi - komisyondan sonra kalan para yeterli mi
            int stockLeft = stockPrice.Stock.Unit - buyingStockUnit;
            if(stockLeft < 0)
            {
                Log.Warning($"StockService-BuyStockAsync Hata: Satın alabileceğiniz kadar hisse senedi yok." +
                    $" Sistemdeki hisse senedi adeti: {stockPrice.Stock.Unit}" +
                    $" Almak istediğini miktar: {buyingStockUnit}");
                throw new Exception("StockService-BuyStockAsync Hata: Satın alabileceğiniz kadar hisse senedi yok." +
                    $" Sistemdeki hisse senedi adeti: {stockPrice.Stock.Unit}" +
                    $" Almak istediğini miktar: {buyingStockUnit}");
            }

            float transactionMoney = stockPrice.Price * buyingStockUnit;
            float commissionAmount = transactionMoney * ((float)mainBoard.CommissionRate / 100);
            float remainingMoney = portfolioUser.Portfolio.Balance - transactionMoney - commissionAmount;
            if (remainingMoney < 0)
            {
                Log.Warning($" StockService-BuyStockAsync Hata:" +
                    $" {transactionMoney} TL'lik işlem miktarı için ödemeniz gerek komisyon {commissionAmount} TL'dir." +
                    $" İşlem sonrası hesabınızda kalan tutar {remainingMoney} TL'dir. Bakiye 0'ın altına düşemez!");
                throw new Exception($" StockService-BuyStockAsync Hata:" +
                    $" {transactionMoney} TL'lik işlem miktarı için ödemeniz gerek komisyon {commissionAmount} TL'dir." +
                    $" İşlem sonrası hesabınızda kalan tutar {remainingMoney} TL'dir. Bakiye 0'ın altına düşemez!");
            }

            mainBoard.Balance += commissionAmount; // komisyonu ekle
            float oldPortfolioBalance = portfolioUser.Portfolio.Balance;
            portfolioUser.Portfolio.Balance = portfolioUser.Portfolio.Balance - transactionMoney - commissionAmount; // ücreti al. komisyonu al.
            if(portfolioStock == null)
            {
                portfolioStock = new PortfolioStock
                {
                    PortfolioId = portfolioUser.PortfolioId,
                    StockId = stockPrice.Stock.Id,
                    Unit = buyingStockUnit                
                };
                await _stockMarketDbContext.PortfolioStock.AddAsync(portfolioStock);
            }
            else
            {
                portfolioStock.Unit += buyingStockUnit;  // portföyümseki ilgili hisse adeti arttırıldı.
            }
            stockPrice.Stock.Unit = stockPrice.Stock.Unit - buyingStockUnit; // sistemdeki hisse senedi adeti eksiltildi.

            //veri tabanına detaylı kaydı ekleme.
            await _stockMarketDbContext.StockBuyAndSale.AddAsync(new StockBuyAndSale 
            { 
                PortfolioId= portfolioUser.PortfolioId,
                StockId= stockPrice.Stock.Id,
                Unit = buyingStockUnit,
                Status = "Buy", //alım demek
                Price = stockPrice.Price, // kaç tl den aldım.
                Date = DateTime.Now
            });

            await _stockMarketDbContext.SaveChangesAsync();

            Log.Warning($"{portfolioStock.Stock.Name} kodlu hisse senedinden," +
                $" {stockPrice.Price} TL fiyatından, {buyingStockUnit} adet satın adlınız." +
                $" İşlem {transactionMoney} TL, komisyon {commissionAmount} TL tuttu." +
                $" Eski bakiyeniz {oldPortfolioBalance} TL.\nYeni bakiyeniz {portfolioUser.Portfolio.Balance} TL." +
                $" Sistemde kalan hisse senedi {portfolioStock.Stock.Unit} adettir.");
            return $"{portfolioStock.Stock.Name} kodlu hisse senedinden,\n" +
                $" {stockPrice.Price} TL fiyatından, {buyingStockUnit} adet satın adlınız.\n" +
                $" İşlem {transactionMoney} TL, komisyon {commissionAmount} TL tuttu.\n" +
                $" Eski bakiyeniz {oldPortfolioBalance} TL.\nYeni bakiyeniz {portfolioUser.Portfolio.Balance} TL.\n" +
                $" Sistemde kalan hisse senedi {portfolioStock.Stock.Unit} adettir.\n";            
            
        }

        public async Task SetStocksAsync(List<StockPriceDTO> stockPriceDTOs)
        {
            /*
             * Api tüm değerleri alamıyor. Big Para bazı hisselerde bazen veri göndermiyor. Veyaz az veri gönderiyor.
             * Yani bir veri güncelleme işlemi yapılmaz, sadece senedin yeni gelen değeri güncel tarih ile eklenir.
             */
            if (stockPriceDTOs != null)
            {
                List<StockPrice> stockPricesFromDB = await _stockMarketDbContext.StockPrices
                    .Include(stockPrices => stockPrices.Stock).ToListAsync();

                foreach (var stockPriceDTO in stockPriceDTOs)
                {
                    StockPrice existingStock = stockPricesFromDB.FirstOrDefault(stockPrice => stockPrice.Stock.Name == stockPriceDTO.StockName);

                    if (existingStock != null)
                    {
                        // Eşleşen bir Stock bulundu, yeni fiyatını StockPrice'e ekle.
                        await _stockMarketDbContext.StockPrices.AddAsync(new StockPrice
                        {
                            StockId = existingStock.Stock.Id,
                            Price = stockPriceDTO.Price,
                            Date = stockPriceDTO.Date,

                        });
                    }
                    else
                    {
                        // Eşleşen bir Stock bulunamadı, yeni bir Stock ekle // fiyatı olmayabilir yani status true gelebilir.
                        await _stockMarketDbContext.StockPrices.AddAsync(new StockPrice
                        {
                            Stock = new Stock { Name = stockPriceDTO.StockName, Unit = 10000, Status = stockPriceDTO.StockStatus },
                            Date = stockPriceDTO.Date,
                            Price = stockPriceDTO.Price,
                        });
                    }
                }       
                await _stockMarketDbContext.SaveChangesAsync();
                Log.Warning($"StockService-SetStocks işlemi çalıştı.");
                return;
            }
            Log.Warning($"StockService-SetStocks Hata: stockPrices is null.");
            throw new Exception($"StockService-SetStocks Hata: stockPrices is null.");
        }

        public async Task<List<StockPriceDTO>> GetAllStocksAsync()
        {
            List<StockPrice> stockPrices = await _stockMarketDbContext.StockPrices
                                            .Include(stockPrices => stockPrices.Stock)
                                            .Where(stockPrice => stockPrice.Stock.Status == false)
                                            .GroupBy(stockPrice => stockPrice.StockId)
                                            .Select(group => group.OrderByDescending(stockPrice => stockPrice.Date).First())
                                            .ToListAsync();
            Log.Warning($"StockService-GetStocks çalıştı.");
            return _mapper.Map<List<StockPriceDTO>>(stockPrices);
        }

        public async Task<List<StockPriceDTO>> GetListStockPricesByNameAsync(string stockName)
        {
            List<StockPrice> stockPrices = await _stockMarketDbContext.StockPrices
                                            .Include(stockPrices => stockPrices.Stock)
                                            .Where(stockPrices => stockPrices.Stock.Status == false && stockPrices.Stock.Name == stockName)
                                            .OrderByDescending(stockPrices => stockPrices.Date).ToListAsync();

            Log.Warning($"StockService-GetListStockPricesByName çalıştı. Sorgulanan Hisse: {stockName}");
            if (!stockPrices.IsNullOrEmpty())
            {
                return _mapper.Map<List<StockPriceDTO>>(stockPrices);
            }
            throw new Exception($"Bu hisse senedi yok veya şuan bu hisse senedinin bilgisi sistemlerimizde mevcut değil. Sorgulanan Hisse: {stockName}");
        }

        public async Task<List<PortfolioStockDTO>> GetOwnedStocksAsync(int userId) 
        {
            PortfolioUser portfolioUser = await _stockMarketDbContext.PortfolioUser.FirstOrDefaultAsync(portfolioUser => portfolioUser.UserId == userId);
            if(portfolioUser == null)
            {
                throw new Exception("Size ait bir portfoy bulunamadı.");
            }
            List<PortfolioStock> portfolioStocks = await _stockMarketDbContext.PortfolioStock
                                                    .Include(portfolioStock => portfolioStock.Portfolio)
                                                    .Include(portfolioStock => portfolioStock.Stock)
                                                    .Where(portfolioStock=> portfolioStock.PortfolioId == portfolioUser.PortfolioId)
                                                    .ToListAsync();
            return _mapper.Map<List<PortfolioStockDTO>>(portfolioStocks);
        }
    }
}

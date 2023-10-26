using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StocksMarketWebAPI.Context;
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

        public async Task SetStocks(List<StockPriceDTO> stockPriceDTOs)
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

        public async Task<List<StockPriceDTO>> GetStocks()
        {
            List<StockPrice> stockPrices = _stockMarketDbContext.StockPrices
                                            .Include(stockPrices => stockPrices.Stock)
                                            .Where(stockPrice => stockPrice.Stock.Status == false)
                                            .GroupBy(stockPrice => stockPrice.StockId)
                                            .Select(group => group.OrderByDescending(stockPrice => stockPrice.Date).First())
                                            .ToList();
            Log.Warning($"StockService-GetStocks çalıştı.");
            return _mapper.Map<List<StockPriceDTO>>(stockPrices);
        }

        public async Task<List<StockPriceDTO>> GetListStockPricesByName(string stockName)
        {
            List<StockPrice> stockPrices = _stockMarketDbContext.StockPrices
                                            .Include(stockPrices => stockPrices.Stock)
                                            .Where(stockPrices => stockPrices.Stock.Status == false && stockPrices.Stock.Name == stockName)
                                            .OrderByDescending(stockPrices => stockPrices.Date).ToList();

            Log.Warning($"StockService-GetListStockPricesByName çalıştı. Sorgulanan Hisse: {stockName}");
            if (!stockPrices.IsNullOrEmpty())
            {
                return _mapper.Map<List<StockPriceDTO>>(stockPrices);
            }
            throw new Exception($"Bu hisse senedi yok veya şuan bu hisse senedinin bilgisi sistemlerimizde mevcut değil. Sorgulanan Hisse: {stockName}");
        }
    }
}

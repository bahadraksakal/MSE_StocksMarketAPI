using HangFireDbContextLibrary.Context;
using StockMarketDbContextLibrary.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

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

        public async Task ChangedPricesStocks()
        {

        }
    }
}

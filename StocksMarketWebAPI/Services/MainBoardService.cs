using Microsoft.EntityFrameworkCore;
using Serilog;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.Entities;

namespace StocksMarketWebAPI.Services
{
    public class MainBoardService
    {
        private readonly StockMarketDbContext _stockMarketDbContext;

        public MainBoardService(StockMarketDbContext stockMarketDbContext)
        {
            _stockMarketDbContext = stockMarketDbContext;
            
        }
        public async Task<MainBoard> GetMainBoardAsync()
        {
            MainBoard mainBoard = await _stockMarketDbContext.MainBoards.FirstOrDefaultAsync();
            if(mainBoard != null) {
                Log.Warning($"MainBoardService-GetMainBoardAsync: çalıştı. MainBoard Id:{mainBoard.Id}");
                return mainBoard;
            }
            Log.Warning($"MainBoardService-GetMainBoardAsync: MainBoard bulunamadı");
            throw new Exception("MainBoard bulunamadı");
        }

        public async Task<MainBoard> ChangeCommissionRateAsync(int newCommissionRate)
        {
            MainBoard mainBoard = await _stockMarketDbContext.MainBoards.FirstOrDefaultAsync();
            if( mainBoard != null )
            {
                mainBoard.CommissionRate = newCommissionRate;
                await _stockMarketDbContext.SaveChangesAsync();
                Log.Warning($"MainBoardService-ChangeCommissionRateAsync: Main Board Komisyon oranını {mainBoard.CommissionRate} olarak ayarladı.");
                return mainBoard;
            }
            Log.Warning($"MainBoardService-ChangeCommissionRateAsync: MainBoard bulunamadı");
            throw new Exception("MainBoard bulunamadı");
        }


    }
}

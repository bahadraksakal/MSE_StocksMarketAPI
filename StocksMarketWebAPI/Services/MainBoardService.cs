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
        public async Task<MainBoard> GetMainBoard()
        {
            MainBoard mainBoard = await _stockMarketDbContext.MainBoards.FirstOrDefaultAsync();
            return mainBoard;
        }

        public async Task<MainBoard> ChangeCommissionRate(int newCommissionRate)
        {
            MainBoard mainBoard = await _stockMarketDbContext.MainBoards.FirstOrDefaultAsync();
            mainBoard.CommissionRate = newCommissionRate;
            await _stockMarketDbContext.SaveChangesAsync();
            return mainBoard;
        }


    }
}

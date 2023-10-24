using Microsoft.EntityFrameworkCore;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.Entities;

namespace StocksMarketWebAPI.Services
{
    public class MainBoardService
    {
        private readonly StockMarketDbContext _stockMarketDbContext;
        //public static MainBoard MainBoard;

        public MainBoardService(StockMarketDbContext stockMarketDbContext)
        {
            _stockMarketDbContext = stockMarketDbContext;
            
        }
        //public async Task setDefaultMainBoard(int balance,int commissionRate)
        //{
        //    MainBoard = await _stockMarketDbContext.MainBoards.FirstOrDefaultAsync();
        //    if (MainBoard == null)
        //    {
        //        MainBoard = new MainBoard()
        //        {
        //            Balance = balance,
        //            CommissionRate = commissionRate,
        //        };
        //        _stockMarketDbContext.MainBoards.Add(MainBoard);
        //        await _stockMarketDbContext.SaveChangesAsync();
        //    }
        //}
        //public MainBoard GetMainBoard()
        //{
        //    return MainBoard;
        //}

        public async Task<MainBoard> ChangeCommissionRate(int newCommissionRate)
        {
            MainBoard mainBoard = await _stockMarketDbContext.MainBoards.FirstOrDefaultAsync();
            if (mainBoard != null)
            {
                mainBoard.CommissionRate = newCommissionRate;
                await _stockMarketDbContext.SaveChangesAsync();
                return mainBoard;
            }
            throw new Exception("Veri tabanında bir MainBoard bulunamadı.");
        }


    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StockMarketDbContextLibrary.Context;
using StocksMarketWebAPI.DTOs.MainBoardDTOs;
using StockMarketEntitiesLibrary.Entities;

namespace StocksMarketWebAPI.Services
{
    public class MainBoardService
    {
        private readonly StockMarketDbContext _stockMarketDbContext;
        private readonly IMapper _mapper;

        public MainBoardService(StockMarketDbContext stockMarketDbContext, IMapper mapper)
        {
            _stockMarketDbContext = stockMarketDbContext;
            _mapper = mapper;
            
        }
        public async Task<MainBoardDTO> GetMainBoardAsync()
        {
            MainBoard mainBoard = await _stockMarketDbContext.MainBoards.AsNoTracking().FirstOrDefaultAsync();
            if(mainBoard != null) {
                Log.Warning($"MainBoardService-GetMainBoardAsync: çalıştı. MainBoard Id:{mainBoard.Id}");
                return _mapper.Map<MainBoardDTO>(mainBoard);
            }
            Log.Warning($"MainBoardService-GetMainBoardAsync: MainBoard bulunamadı");
            throw new Exception("MainBoard bulunamadı");
        }

        public async Task<MainBoardDTO> ChangeCommissionRateAsync(int newCommissionRate)
        {
            MainBoard mainBoard = await _stockMarketDbContext.MainBoards.FirstOrDefaultAsync();
            if( mainBoard != null )
            {
                mainBoard.CommissionRate = newCommissionRate;
                await _stockMarketDbContext.SaveChangesAsync();
                Log.Warning($"MainBoardService-ChangeCommissionRateAsync: Main Board Komisyon oranını {mainBoard.CommissionRate} olarak ayarladı.");
                return _mapper.Map<MainBoardDTO>(mainBoard);
            }
            Log.Warning($"MainBoardService-ChangeCommissionRateAsync: MainBoard bulunamadı");
            throw new Exception("MainBoard bulunamadı");
        }
    }
}

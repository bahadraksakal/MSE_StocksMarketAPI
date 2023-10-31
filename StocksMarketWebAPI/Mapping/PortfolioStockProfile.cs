using AutoMapper;
using StocksMarketWebAPI.DTOs.PortfolioStockDTOs;
using StockMarketEntitiesLibrary.Entities;

namespace StocksMarketWebAPI.Mapping
{
    public class PortfolioStockProfile:Profile
    {
        public PortfolioStockProfile() 
        {
            CreateMap<PortfolioStock, PortfolioStockDTO>().ForMember(dest => dest.PortfolioBalance, opt => opt.MapFrom(src => src.Portfolio.Balance))
                                                    .ForMember(dest => dest.StockName, opt => opt.MapFrom(src => src.Stock.Name))
                                                    .ForMember(dest => dest.StockUnit, opt => opt.MapFrom(src => src.Stock.Unit))
                                                    .ForMember(dest => dest.StockStatus, opt => opt.MapFrom(src => src.Stock.Status));
            CreateMap<PortfolioStockDTO, PortfolioStock>()
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => new Stock
            {
                Id = src.StockId,
                Name = src.StockName,
                Unit = (int)src.StockUnit,
                Status = (bool)src.StockStatus
            }))
            .ForMember(dest => dest.Portfolio, opt => opt.MapFrom(src => new Portfolio
            {
                Id = src.PortfolioId,
                Balance = (float)src.PortfolioBalance
            }));

        }
    }
}

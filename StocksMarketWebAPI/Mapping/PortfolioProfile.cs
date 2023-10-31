using AutoMapper;
using StocksMarketWebAPI.DTOs.PortfolioDTOs;
using StockMarketEntitiesLibrary.Entities;

namespace StocksMarketWebAPI.Mapping
{
    public class PortfolioProfile : Profile
    {
        public PortfolioProfile()
        {
            CreateMap<Portfolio,PortfolioDTO>();
            CreateMap<PortfolioDTO, Portfolio>();
        }
    }
}

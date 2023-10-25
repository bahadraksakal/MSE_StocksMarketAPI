using AutoMapper;
using StocksMarketWebAPI.DTOs.PortfolioDTOs;
using StocksMarketWebAPI.Entities;

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

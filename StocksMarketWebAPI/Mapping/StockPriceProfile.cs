using AutoMapper;
using StocksMarketWebAPI.DTOs.StockPriceDTOs;
using StocksMarketWebAPI.Entities;

namespace StocksMarketWebAPI.Mapping
{
    public class StockPriceProfile:Profile
    {
        public StockPriceProfile() 
        {
            CreateMap<StockPrice, StockPriceDTO>()
            .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.Stock.Id))
            .ForMember(dest => dest.StockName, opt => opt.MapFrom(src => src.Stock.Name))
            .ForMember(dest => dest.StockUnit, opt => opt.MapFrom(src => src.Stock.Unit))
            .ForMember(dest => dest.StockStatus, opt => opt.MapFrom(src => src.Stock.Status))
            .ReverseMap();
        }
    }
}

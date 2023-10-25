using AutoMapper;
using StocksMarketWebAPI.DTOs.StockDTOs;
using StocksMarketWebAPI.Entities;

namespace StocksMarketWebAPI.Mapping
{
    public class StockProfile:Profile
    {
        public StockProfile() 
        {
            CreateMap<Stock, StockDTO>();
            CreateMap<StockDTO, Stock>();
        }
    }
}

using AutoMapper;
using StocksMarketWebAPI.DTOs.StockDTOs;
using StockMarketEntitiesLibrary.Entities;

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

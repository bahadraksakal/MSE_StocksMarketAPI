using AutoMapper;
using StocksMarketWebAPI.DTOs.MoneyCardDTOs;
using StockMarketEntitiesLibrary.Entities;

namespace StocksMarketWebAPI.Mapping
{
    public class MoneyCardProfile:Profile
    {
        public MoneyCardProfile()
        {
            CreateMap<MoneyCard, MoneyCardDTO>();
            CreateMap<MoneyCardDTO, MoneyCard>();
        }
    }
}

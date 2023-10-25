using AutoMapper;
using StocksMarketWebAPI.DTOs.MoneyCardDTOs;
using StocksMarketWebAPI.Entities;

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

using AutoMapper;
using StocksMarketWebAPI.DTOs.UserDTOs;
using StockMarketEntitiesLibrary.Entities;

namespace StocksMarketWebAPI.Mapping
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<UserDTO, User>();
        }
    }
}

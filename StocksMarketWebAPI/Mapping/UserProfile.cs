using AutoMapper;
using StocksMarketWebAPI.DTOs.UserDTOs;
using StocksMarketWebAPI.Entities;

namespace StocksMarketWebAPI.Mapping
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}

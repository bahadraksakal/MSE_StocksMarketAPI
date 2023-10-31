using AutoMapper;
using StocksMarketWebAPI.DTOs.PortfolioUserDTOs;
using StockMarketEntitiesLibrary.Entities;

namespace StocksMarketWebAPI.Mapping
{
    public class PortfolioUserProfile:Profile
    {
        public PortfolioUserProfile()
        {
            CreateMap<PortfolioUser, PortfolioUserDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.UserTel, opt => opt.MapFrom(src => src.User.Tel))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.User.Role))
            .ForMember(dest => dest.PortfolioBalance, opt => opt.MapFrom(src => src.Portfolio.Balance));

            CreateMap<PortfolioUserDTO, PortfolioUser>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User
            {
                Name = src.UserName,
                Email = src.UserEmail,
                Tel = src.UserTel,
                Role = src.UserRole
            }))
            .ForMember(dest => dest.Portfolio, opt => opt.MapFrom(src => new Portfolio
            {
                Balance = (float)src.PortfolioBalance
            }));
        }
    }
}

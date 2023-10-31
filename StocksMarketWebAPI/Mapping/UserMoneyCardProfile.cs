using AutoMapper;
using StocksMarketWebAPI.DTOs.UserMoneyCardDTOs;
using StockMarketEntitiesLibrary.Entities;

namespace StocksMarketWebAPI.Mapping
{
    public class UserMoneyCardProfile:Profile
    {
        public UserMoneyCardProfile()
        {
            CreateMap<UserMoneyCard, UserMoneyCardDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.UserTel, opt => opt.MapFrom(src => src.User.Tel))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.User.Role))
            .ForMember(dest => dest.MoneyCardBalance, opt => opt.MapFrom(src => src.MoneyCard.Balance))
            .ForMember(dest => dest.MoneyCardCreationDate, opt => opt.MapFrom(src => src.MoneyCard.CreationDate))
            .ForMember(dest => dest.MoneyCardUsedDate, opt => opt.MapFrom(src => src.MoneyCard.UsedDate));

            CreateMap<UserMoneyCardDTO, UserMoneyCard>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User
            {
                Name = src.UserName,
                Email = src.UserEmail,
                Tel = src.UserTel,
                Role = src.UserRole
            }))
            .ForMember(dest => dest.MoneyCard, opt => opt.MapFrom(src => new MoneyCard
            {
                Balance = (float)src.MoneyCardBalance,
                CreationDate = (DateTime)src.MoneyCardCreationDate,
                UsedDate = src.MoneyCardUsedDate
            }));

            CreateMap<UserMoneyCard, UserMoneyCardAllCardListLiteDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.MoneyCardId, opt => opt.MapFrom(src => src.MoneyCard.Id))
            .ForMember(dest => dest.MoneyCardBalance, opt => opt.MapFrom(src => src.MoneyCard.Balance))
            .ForMember(dest => dest.MoneyCardCreationDate, opt => opt.MapFrom(src => src.MoneyCard.CreationDate))
            .ForMember(dest => dest.MoneyCardUsedDate, opt => opt.MapFrom(src => src.MoneyCard.UsedDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<UserMoneyCardAllCardListLiteDTO, UserMoneyCard>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User
            {
                Name = src.UserName,
            }))
            .ForMember(dest => dest.MoneyCard, opt => opt.MapFrom(src => new MoneyCard
            {
                Balance = src.MoneyCardBalance,
                CreationDate = src.MoneyCardCreationDate,
                UsedDate = src.MoneyCardUsedDate
            }));

            CreateMap<UserMoneyCard, UserMoneyCardOwnedCardListLiteDTO>()
            .ForMember(dest => dest.MoneyCardId, opt => opt.MapFrom(src => src.MoneyCard.Id))
            .ForMember(dest => dest.MoneyCardBalance, opt => opt.MapFrom(src => src.MoneyCard.Balance))
            .ForMember(dest => dest.MoneyCardCreationDate, opt => opt.MapFrom(src => src.MoneyCard.CreationDate))
            .ForMember(dest => dest.MoneyCardUsedDate, opt => opt.MapFrom(src => src.MoneyCard.UsedDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}

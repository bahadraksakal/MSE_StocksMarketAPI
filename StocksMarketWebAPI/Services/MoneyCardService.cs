using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.DTOs.UserMoneyCardDTOs;
using StocksMarketWebAPI.Entities;

namespace StocksMarketWebAPI.Services
{
    public class MoneyCardService
    {
        private readonly StockMarketDbContext _stockMarketDbContext;
        private readonly IMapper _mapper;

        public MoneyCardService(StockMarketDbContext stockMarketDbContext, IMapper mapper) 
        { 
            _stockMarketDbContext = stockMarketDbContext;
            _mapper = mapper;
        }
        public async Task<List<UserMoneyCardOwnedCardListLiteDTO>> GetOwnedMoneyCardAsync(int userId)
        {
            List<UserMoneyCard> userMoneyCards = await _stockMarketDbContext.UsersMoneyCard
                .Include(userMoneyCard => userMoneyCard.User)
                .Include(userMoneyCard => userMoneyCard.MoneyCard)
                .Where(userMoneyCard => userMoneyCard.UserId==userId)
                .ToListAsync();
            if(userMoneyCards.IsNullOrEmpty())
            {
                throw new Exception("Tanımlı hiçbir para kartınız yok.");
            }
            return _mapper.Map<List<UserMoneyCardOwnedCardListLiteDTO>>(userMoneyCards);
        }
        public async Task<List<UserMoneyCardAllCardListLiteDTO>> GetAllMoneyCardsAsync()
        {
            List<UserMoneyCard> userMoneyCards = await _stockMarketDbContext.UsersMoneyCard
                .Include(userMoneyCard => userMoneyCard.User)
                .Include(userMoneyCard => userMoneyCard.MoneyCard)
                .OrderByDescending(userMoneyCard => userMoneyCard.MoneyCard.CreationDate)
                .ToListAsync();
            if (userMoneyCards.IsNullOrEmpty())
            {
                throw new Exception("Hiçbir kullanıcı adına Tanımlı hiçbir para kartınız yok.");
            }
            return _mapper.Map<List<UserMoneyCardAllCardListLiteDTO>>(userMoneyCards);
        }
        public async Task<UserMoneyCardDTO> AddMoneyCardAsync(int userId,int balance)
        {
            User userTemp=await _stockMarketDbContext.Users.FirstOrDefaultAsync(user=>user.Id == userId);
            if(userTemp!=null){
                UserMoneyCard newUserMoneyCard = new UserMoneyCard() { 
                    User=userTemp,
                    MoneyCard= new MoneyCard() { Balance=balance ,CreationDate=DateTime.Now},  
                    Status=false
                };
                await _stockMarketDbContext.UsersMoneyCard.AddAsync(newUserMoneyCard);
                await _stockMarketDbContext.SaveChangesAsync();
                Log.Warning($"MoneyCardService:AddMoneyCardAsync: admin, {newUserMoneyCard.User.Id} id'li {newUserMoneyCard.User.Name} adındaki kullanıcıya" +
                    $" -{newUserMoneyCard.MoneyCard.Balance}- değerinde para kartı tanımladı.");
                return _mapper.Map<UserMoneyCardDTO>(newUserMoneyCard);            
            }
            Log.Warning($"MoneyCardService:AddMoneyCardAsync: {userId} id'sine sahip bir kullanıcı bulunamadı");
            throw new Exception($"{userId} id'sine sahip bir kullanıcı bulunamadı");
        }
        public async Task<UserMoneyCardDTO> UseMoneyCardAsync(int userId, int moneyCardId)
        {
            UserMoneyCard userMoneyCard=await _stockMarketDbContext.UsersMoneyCard
                .Include(userMoneyCards=>userMoneyCards.MoneyCard)
                .Include(userMoneyCards => userMoneyCards.User)
                .ThenInclude(users=>users.PortfolioUser)
                .ThenInclude(portfolioUsers=>portfolioUsers.Portfolio)
                .FirstOrDefaultAsync(userMoneyCard=>
                    userMoneyCard.UserId == userId
                    && userMoneyCard.MoneyCardId==moneyCardId);
            if (userMoneyCard != null)
            {
                if (userMoneyCard.Status == false)
                {
                    userMoneyCard.Status = true;
                    userMoneyCard.User.PortfolioUser.Portfolio.Balance += userMoneyCard.MoneyCard.Balance;
                    userMoneyCard.MoneyCard.UsedDate= DateTime.Now;
                    await _stockMarketDbContext.SaveChangesAsync();
                    Log.Warning($"MoneyCardService:UseMoneyCardAsync: {userMoneyCard.User.Id} id'li {userMoneyCard.User.Name} adındaki kullanıcı" +
                        $"{userMoneyCard.MoneyCard.Id} id'li {userMoneyCard.MoneyCard.Balance} değerindeki para kartını kullandı.");
                    return _mapper.Map<UserMoneyCardDTO>(userMoneyCard);
                }
                Log.Warning($"MoneyCardService:UseMoneyCardAsync: moneyCardId:{userMoneyCard.MoneyCardId} - UserId:{userMoneyCard.UserId} değerlerine sahip bir UserMoneyCard zaten kullanılmış.");
                throw new Exception($"moneyCardId:{userMoneyCard.MoneyCardId} - UserId:{userMoneyCard.UserId} değerlerine sahip bir UserMoneyCard zaten kullanılmış.");
            }
            Log.Warning($"MoneyCardService:UseMoneyCardAsync: moneyCardId:{moneyCardId} - UserId:{userId} değerlerine sahip bir UserMoneyCard bulunamadı.");
            throw new Exception($"moneyCardId:{moneyCardId} - UserId:{userId} değerlerine sahip bir UserMoneyCard bulunamadı.");
        }
    }
}

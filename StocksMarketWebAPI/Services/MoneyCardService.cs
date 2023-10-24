using Microsoft.EntityFrameworkCore;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.Entities;

namespace StocksMarketWebAPI.Services
{
    public class MoneyCardService
    {
        private readonly StockMarketDbContext _stockMarketDbContext;

        public MoneyCardService(StockMarketDbContext stockMarketDbContext) 
        { 
            _stockMarketDbContext = stockMarketDbContext;
        }
        public async Task<UserMoneyCard> AddMoneyCardAsync(int userId,int balance)
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
                return newUserMoneyCard;            
            }
            throw new Exception($"{userId} id'sine sahip bir kullanıcı bulunamadı");
        }
        public async Task<UserMoneyCard> UseMoneyCardAsync(int userId, int moneyCardId)
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
                    await _stockMarketDbContext.SaveChangesAsync();
                    return userMoneyCard;
                }
                throw new Exception($"moneyCardId:{userMoneyCard.MoneyCardId} - UserId:{userMoneyCard.UserId} değerlerine sahip bir UserMoneyCard zaten kullanılmış.");
            }
            throw new Exception($"moneyCardId:{moneyCardId} - UserId:{userId} değerlerine sahip bir UserMoneyCard bulunamadı.");
        }
    }
}

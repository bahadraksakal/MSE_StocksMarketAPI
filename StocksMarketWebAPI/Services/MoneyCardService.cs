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
        public async Task<UserMoneyCard> AddMoneyCard(int userId,int balance)
        {
            User userTemp=await _stockMarketDbContext.Users.SingleOrDefaultAsync(user=>user.Id == userId);
            if(userTemp!=null){
                UserMoneyCard newUserMoneyCard = new UserMoneyCard() { 
                    User=userTemp,
                    MoneyCard= new MoneyCard() { Balance=balance ,CreationDate=DateTime.Now},  
                    Status=false
                };
                _stockMarketDbContext.UsersMoneyCard.Add(newUserMoneyCard);
                await _stockMarketDbContext.SaveChangesAsync();
                return newUserMoneyCard;            
            }
            throw new Exception("Bu id'ye sahip kullanıcı bulunamadı");
        }
    }
}

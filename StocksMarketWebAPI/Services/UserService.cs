using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.Entities;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace StocksMarketWebAPI.Services
{
    public class UserService
    {
        private readonly StockMarketDbContext _stockMarketDbContext;

        public UserService(StockMarketDbContext stockMarketDbContext){
            _stockMarketDbContext = stockMarketDbContext;        
        }
        public async Task<User> AddUser(string userName, string userPassword, string userEmail, string userTel, string userRole)
        {
            User newUser = new User() { Name = userName, Password = userPassword, Email = userEmail, Tel = userTel, Role = userRole };
            PortfolioUser userPortfolio = new PortfolioUser
            {
                User = newUser,
                Portfolio = new Portfolio() { Balance = 0 }
            };
            _stockMarketDbContext.PortfolioUser.Add(userPortfolio);
            await _stockMarketDbContext.SaveChangesAsync();
            return newUser;

        }
        public async Task<User> UpdateUserRoleById(int id,string role)
        {
            User userChanged=_stockMarketDbContext.Users.SingleOrDefault(user => user.Id == id);
            if(userChanged != null)
            {
               userChanged.Role = role;
               await _stockMarketDbContext.SaveChangesAsync();
               return userChanged;
            }
            throw new Exception("Bu id'ye sahip kullanıcı bulunamadı");
        }
        public async Task<PortfolioUser> UpdateUserPortfolioBalanceById(int id,int newBalance)
        {
            PortfolioUser portfolioChanged = await _stockMarketDbContext.PortfolioUser.SingleOrDefaultAsync(portfolioUser => portfolioUser.UserId == id);
            if (portfolioChanged != null)
            {
                portfolioChanged.Portfolio.Balance = newBalance;
                await _stockMarketDbContext.SaveChangesAsync();
                return portfolioChanged;
            }
            throw new Exception("Bu id'ye sahip kullanıcı bulunamadı");
        }
        public async Task<bool> isAdmin(string idStr)
        {
            int id = int.Parse(idStr);
            User admin= await _stockMarketDbContext.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Id == id);
            return (admin.Role == "Admin") ? true : false;
        }
        public async Task<bool> isAdmin(int id)
        {
            User admin = await _stockMarketDbContext.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Id == id);
            return (admin.Role == "Admin") ? true : false;
        }
    }
}

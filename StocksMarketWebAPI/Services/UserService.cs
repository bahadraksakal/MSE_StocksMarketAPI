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
        public async Task<User> AddUserAsync(string userName, string userPassword, string userEmail, string userTel, string userRole)
        {
            User newUser = new User() { Name = userName, Password = userPassword, Email = userEmail, Tel = userTel, Role = userRole };
            PortfolioUser userPortfolio = new PortfolioUser
            {
                User = newUser,
                Portfolio = new Portfolio() { Balance = 0 }
            };
            await _stockMarketDbContext.PortfolioUser.AddAsync(userPortfolio);
            await _stockMarketDbContext.SaveChangesAsync();
            Log.Warning($"Kullanıcı başarıyla eklendi: " +
                $"Name:{userPortfolio.User.Name} - Role:{userPortfolio.User.Role} - Bakiye: {userPortfolio.Portfolio.Balance}");
            return newUser;

        }
        public async Task<User> UpdateUserRoleByIdAsync(int id,string role)
        {
            User userChanged= await _stockMarketDbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
            if(userChanged != null)
            {
               userChanged.Role = role;
               await _stockMarketDbContext.SaveChangesAsync();

                Log.Warning($"{userChanged.Id} id'li {userChanged.Name} adındaki kullanıcının rolünü -{userChanged.Role}- olarak değiştirdi.");
                return userChanged;
            }
            Log.Warning($"{id} id'li kullanıcı bulunamadı");
            throw new Exception($"{id} id'li kullanıcı bulunamadı");
        }
        public async Task<PortfolioUser> UpdateUserPortfolioBalanceByIdAsync(int id,int newBalance)
        {
            PortfolioUser portfolioChanged = await _stockMarketDbContext.PortfolioUser
                .Include(portfolioUser=>portfolioUser.Portfolio)
                .Include(portfolioUser => portfolioUser.User)
                .FirstOrDefaultAsync(portfolioUser => portfolioUser.UserId == id);
            if (portfolioChanged != null)
            {
                portfolioChanged.Portfolio.Balance = newBalance;
                await _stockMarketDbContext.SaveChangesAsync();

                Log.Warning($"{portfolioChanged.UserId} id'li {portfolioChanged.User.Name} adındaki kullanıcının bakiyesini -{portfolioChanged.Portfolio.Balance}- olarak değiştirdi.");
                return portfolioChanged;
            }
            Log.Warning($"{id} id'li kullanıcı bulunamadı");
            throw new Exception($"{id} id'li kullanıcı bulunamadı");
        }
        public async Task<bool> isAdminAsync(string idStr)
        {
            int id = int.Parse(idStr);
            User admin= await _stockMarketDbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);
            return (admin.Role == "Admin") ? true : false;
        }
        public async Task<bool> isAdminAsync(int id)
        {
            User admin = await _stockMarketDbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);
            return (admin.Role == "Admin") ? true : false;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.Entities;
using System.Text.Json.Serialization;
using System.Text.Json;
using AutoMapper;
using StocksMarketWebAPI.DTOs.UserDTOs;
using StocksMarketWebAPI.DTOs.PortfolioUserDTOs;

namespace StocksMarketWebAPI.Services
{
    public class UserService
    {
        private readonly StockMarketDbContext _stockMarketDbContext;
        private readonly IMapper _mapper;

        public UserService(StockMarketDbContext stockMarketDbContext,IMapper mapper){
            _stockMarketDbContext = stockMarketDbContext;        
            _mapper = mapper;
        }
        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            List<User> users= _stockMarketDbContext.Users.AsNoTracking().ToList();
            return _mapper.Map<List<UserDTO>>(users);
        }
        public async Task<List<PortfolioUserDTO>> GetAllUsersWithDetailAsync()
        {
            List<PortfolioUser> portfolioUsers = _stockMarketDbContext.PortfolioUser.AsNoTracking()
                .Include(portfolioUsers => portfolioUsers.Portfolio)
                .Include(portfolioUsers => portfolioUsers.User).ToList();
            return _mapper.Map<List<PortfolioUserDTO>>(portfolioUsers);
        }
        public async Task<UserDTO> AddUserAsync(string userName, string userPassword, string userEmail, string userTel, string userRole)
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
            return _mapper.Map<UserDTO>(newUser);

        }
        public async Task<UserDTO> UpdateUserRoleByIdAsync(int id,string role)
        {
            User userChanged= await _stockMarketDbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
            if(userChanged != null)
            {
               userChanged.Role = role;
               await _stockMarketDbContext.SaveChangesAsync();

                Log.Warning($"{userChanged.Id} id'li {userChanged.Name} adındaki kullanıcının rolünü -{userChanged.Role}- olarak değiştirdi.");
                return _mapper.Map<UserDTO>(userChanged);
            }
            Log.Warning($"{id} id'li kullanıcı bulunamadı");
            throw new Exception($"{id} id'li kullanıcı bulunamadı");
        }
        public async Task<PortfolioUserDTO> UpdateUserPortfolioBalanceByIdAsync(int id,int newBalance)
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
                return _mapper.Map<PortfolioUserDTO>(portfolioChanged);
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

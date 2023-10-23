using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.Entities;
using StocksMarketWebAPI.Services;
using System.Data;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StocksMarketWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly StockMarketDbContext _stockMarketDbContext;
        private readonly UserService _userService;

        public UserController(ILogger<UserController> logger, StockMarketDbContext stockMarketDbContext, UserService userService)
        {
            this._logger = logger;
            _stockMarketDbContext = stockMarketDbContext;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(string userName, string userPassword, string userEmail, string userTel, string userRole)
        {
            try
            {
                User newUser = await _userService.AddUser(userName, userPassword, userEmail, userTel, userRole);
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                return new ContentResult
                {
                    Content = JsonSerializer.Serialize(newUser, options),
                    ContentType = "application/json",
                    StatusCode = 201 // Başarı durumu
                };

            }
            catch (Exception ex)
            {
                Log.Warning("Veri tabanı hatası:" + ex.InnerException.Message);
                return BadRequest("Veri tabanı hatası:" + ex.InnerException.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}/{role}")]
        public async Task<IActionResult> ChangeStatusUser(int id,string role)
        {
            try
            {
                string adminId = User.FindFirstValue("userId");
                bool isAdmin = await _userService.isAdmin(adminId);
                if (isAdmin)
                {
                    User changedUser = await _userService.UpdateUserRoleById(id, role);
                    Log.Warning($"{User.FindFirstValue("userId")} - {User.FindFirstValue("userName")}  bilgilerine sahip admin, {changedUser.Id} id'li {changedUser.Name} adındaki kullanıcının rolünü -{changedUser.Role}- olarak değiştirdi.");
                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    };
                    return new ContentResult
                    {
                        Content = JsonSerializer.Serialize(changedUser, options),
                        ContentType = "application/json",
                        StatusCode = 200 // Başarı durumu
                    };
                }
                return BadRequest("UserController:ChangeStatusUser hata: Admin değilsiniz.");
            }
            catch (Exception ex)
            {
                Log.Warning("UserController:ChangeStatusUser hata:" + ex.Message);
                return BadRequest("UserController:ChangeStatusUser hata:" + ex.Message);
            }            
        }

        [Authorize]
        [HttpGet("{id}/{balance}")]
        public async Task<IActionResult> ChangePortfolioBalanceUser(int id,int balance)
        {
            try
            {
                string adminId = User.FindFirstValue("userId");
                bool isAdmin = await _userService.isAdmin(adminId);
                if (isAdmin)
                {
                    PortfolioUser changedPortfolio = await _userService.UpdateUserPortfolioBalanceById(id, balance);
                    Log.Warning($"{User.FindFirstValue("userId")} - {User.FindFirstValue("userName")}  bilgilerine sahip admin, {changedPortfolio.UserId} id'li {changedPortfolio.User.Name} adındaki kullanıcının bakiyesini -{changedPortfolio.Portfolio.Balance}- olarak değiştirdi.");
                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    };
                    return new ContentResult
                    {
                        Content = JsonSerializer.Serialize(changedPortfolio, options),
                        ContentType = "application/json",
                        StatusCode = 200 // Başarı durumu
                    };
                }
                return BadRequest("UserController:ChangePortfolioBalanceUser hata: Admin değilsiniz.");
            }
            catch (Exception ex)
            {
                Log.Warning("Veri tabanı hatası:" + ex.InnerException.Message);
                return BadRequest("Veri tabanı hatası:" + ex.InnerException.Message);
            }
        }
    }
}

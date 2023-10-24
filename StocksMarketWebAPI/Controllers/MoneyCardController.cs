using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.Entities;
using StocksMarketWebAPI.Services;
using System.Data;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace StocksMarketWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MoneyCardController : ControllerBase
    {
        private readonly StockMarketDbContext _stockMarketDbContext;
        private readonly UserService _userService;
        private readonly MoneyCardService _moneyCardService;
        public MoneyCardController(StockMarketDbContext stockMarketDbContext, UserService userService, MoneyCardService moneyCardService)
        {
            _stockMarketDbContext = stockMarketDbContext;
            _userService = userService;
            _moneyCardService = moneyCardService;
        }

        [HttpGet("{userId}/{balance}")]
        public async Task<IActionResult> AddMoneyCard(int userId,int balance)
        {
            try
            {
                string adminId = User.FindFirstValue("userId");
                bool isAdmin = await _userService.isAdminAsync(adminId);
                if (isAdmin)
                {
                    UserMoneyCard newUserMoneyCard = await _moneyCardService.AddMoneyCardAsync(userId,balance);
                    Log.Warning($"{User.FindFirstValue("userId")} - {User.FindFirstValue("userName")}  bilgilerine sahip admin, {newUserMoneyCard.User.Id} id'li {newUserMoneyCard.User.Name} adındaki kullanıcıya -{newUserMoneyCard.MoneyCard.Balance}- değerinde para kartı tanımladı.");
                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    };
                    return new ContentResult
                    {
                        Content = JsonSerializer.Serialize(newUserMoneyCard, options),
                        ContentType = "application/json",
                        StatusCode = 201 // Başarı durumu
                    };
                }
                return BadRequest("MoneyCardController:AddMoneyCard hata: Admin değilsiniz.");
            }
            catch (Exception ex)
            {
                Log.Warning("MoneyCardController:AddMoneyCard hata:" + ex.Message);
                return BadRequest("MoneyCardController:AddMoneyCard hata:" + ex.Message);
            }
        }

        [HttpGet("{moneyCardId}")]
        public async Task<IActionResult> UseMoneyCard(int moneyCardId)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("userId"));
                UserMoneyCard newUserMoneyCard = await _moneyCardService.UseMoneyCardAsync(userId, moneyCardId);
                Log.Warning($" {newUserMoneyCard.User.Id} id'li {newUserMoneyCard.User.Name} adındaki kullanıcı" +
                    $"{newUserMoneyCard.MoneyCard.Id} id'li {newUserMoneyCard.MoneyCard.Balance} değerindeki para kartını kullandı.");
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                return new ContentResult
                {
                    Content = JsonSerializer.Serialize(newUserMoneyCard, options),
                    ContentType = "application/json",
                    StatusCode = 200 // Başarı durumu
                };                
            }
            catch (Exception ex)
            {
                Log.Warning("MoneyCardController:UseMoneyCard hata:" + ex.Message);
                return BadRequest("MoneyCardController:UseMoneyCard hata:" + ex.Message);
            }
        }
    }
}

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
        private readonly UserService _userService;
        private readonly MoneyCardService _moneyCardService;
        public MoneyCardController(UserService userService, MoneyCardService moneyCardService)
        {
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
                return BadRequest("MoneyCardController:UseMoneyCard hata:" + ex.Message);
            }
        }
    }
}

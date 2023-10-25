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
using StocksMarketWebAPI.DTOs.UserMoneyCardDTOs;

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
                    UserMoneyCardDTO newUserMoneyCard = await _moneyCardService.AddMoneyCardAsync(userId,balance);
                    return StatusCode(StatusCodes.Status201Created, newUserMoneyCard);
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
                UserMoneyCardDTO usedUserMoneyCard = await _moneyCardService.UseMoneyCardAsync(userId, moneyCardId);
                return Ok(usedUserMoneyCard);           
            }
            catch (Exception ex)
            {
                return BadRequest("MoneyCardController:UseMoneyCard hata:" + ex.Message);
            }
        }
    }
}

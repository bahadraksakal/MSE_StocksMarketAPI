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
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(string userName, string userPassword, string userEmail, string userTel, string userRole)
        {
            try
            {
                User newUser = await _userService.AddUserAsync(userName, userPassword, userEmail, userTel, userRole);
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
                return BadRequest("Veri tabanı hatası:" + ex.InnerException.Message);
            }
        }
        
        [HttpGet("{id}/{role}")]
        public async Task<IActionResult> ChangeStatusUser(int id,string role)
        {
            try
            {
                string adminId = User.FindFirstValue("userId");
                bool isAdmin = await _userService.isAdminAsync(adminId);
                if (isAdmin)
                {
                    User changedUser = await _userService.UpdateUserRoleByIdAsync(id, role);
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
                return BadRequest("UserController:ChangeStatusUser hata:" + ex.Message);
            }            
        }

        [HttpGet("{id}/{balance}")]
        public async Task<IActionResult> ChangePortfolioBalanceUser(int id,int balance)
        {
            try
            {
                string adminId = User.FindFirstValue("userId");
                bool isAdmin = await _userService.isAdminAsync(adminId);
                if (isAdmin)
                {
                    PortfolioUser changedPortfolio = await _userService.UpdateUserPortfolioBalanceByIdAsync(id, balance);
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
                return BadRequest("Veri tabanı hatası:" + ex.InnerException.Message);
            }
        }
    }
}

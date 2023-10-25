using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.DTOs.PortfolioUserDTOs;
using StocksMarketWebAPI.DTOs.UserDTOs;
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
        public async Task<IActionResult> AddUser([FromBody]UserDTO user)
        {
            try
            {
                UserDTO newUser = await _userService.AddUserAsync(user.Name, user.Password, user.Email, user.Tel, user.Role);
                return StatusCode(StatusCodes.Status201Created, newUser);

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
                    UserDTO changedUser = await _userService.UpdateUserRoleByIdAsync(id, role);
                    return Ok(changedUser);
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
                    PortfolioUserDTO changedPortfolio = await _userService.UpdateUserPortfolioBalanceByIdAsync(id, balance);
                    return Ok(changedPortfolio);
                }
                return BadRequest("UserController:ChangePortfolioBalanceUser hata: Admin değilsiniz.");
            }
            catch (Exception ex)
            {
                return BadRequest("UserController:ChangePortfolioBalanceUser hata:" + ex.Message);
            }
        }
    }
}

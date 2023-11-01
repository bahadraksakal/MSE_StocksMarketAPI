using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksMarketWebAPI.DTOs.PortfolioUserDTOs;
using StocksMarketWebAPI.DTOs.UserDTOs;
using StockMarketEntitiesLibrary.Entities;
using StocksMarketWebAPI.Services;
using System.Security.Claims;

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
        public async Task<IActionResult> AddUser([FromBody] UserDTO user)
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

        [HttpPatch("{id}/{role}")]
        public async Task<IActionResult> ChangeStatusUser(int id, string role)
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

        [HttpPatch("{id}/{balance}")]
        public async Task<IActionResult> ChangePortfolioBalanceUser(int id, int balance)
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

        [Authorize(Policy = "CustomAdminPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                List<UserDTO> userDTOs = await _userService.GetAllUsersAsync();
                return Ok(userDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest("StockController:GetUsershata:" + ex.Message);
            }
        }
        [Authorize(Policy = "CustomAdminPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersWithDeatil()
        {
            try
            {
                List<PortfolioUserDTO> portfolioUserDTOs = await _userService.GetAllUsersWithDetailAsync();
                return Ok(portfolioUserDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest("StockController:GetUsershata:" + ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksMarketWebAPI.Entities;
using StocksMarketWebAPI.Services;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using StocksMarketWebAPI.DTOs.MainBoardDTOs;

namespace StocksMarketWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MainBoardController :ControllerBase
    {
        private readonly MainBoardService _mainBoardService;
        private readonly UserService _userService;

        public MainBoardController(MainBoardService mainBoardService, UserService userService)
        {
            _mainBoardService = mainBoardService;
            _userService = userService;
        }

        [HttpGet("{commissionRate}")]
        public async Task<IActionResult> ChangeCommissionRate(int commissionRate)
        {
            try
            {
                string adminId = User.FindFirstValue("userId");
                bool isAdmin = await _userService.isAdminAsync(adminId);
                if (isAdmin)
                {
                    MainBoardDTO mainBoard = await _mainBoardService.ChangeCommissionRateAsync(commissionRate);
                    return Ok(mainBoard);
                }
                return BadRequest("MainBoardController:ChangeCommissionRate hata: Admin değilsiniz.");
            }
            catch (Exception ex)
            {
                return BadRequest("MainBoardController:ChangeCommissionRate hata:" + ex.Message);
            }
        }

    }
}

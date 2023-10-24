using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.Entities;
using StocksMarketWebAPI.Services;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;

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
                bool isAdmin = await _userService.isAdmin(adminId);
                if (isAdmin)
                {
                    MainBoard mainBoard = await _mainBoardService.ChangeCommissionRate(commissionRate);
                    Log.Warning($"{User.FindFirstValue("userId")} - {User.FindFirstValue("userName")}  bilgilerine sahip admin," +
                        $"Main Board Komisyon oranını {mainBoard.CommissionRate} olarak ayarladı.");
                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    };
                    return new ContentResult
                    {
                        Content = JsonSerializer.Serialize(mainBoard, options),
                        ContentType = "application/json",
                        StatusCode = 201 // Başarı durumu
                    };
                }
                return BadRequest("MainBoardController:ChangeCommissionRate hata: Admin değilsiniz.");
            }
            catch (Exception ex)
            {
                Log.Warning("MainBoardController:ChangeCommissionRate hata:" + ex.Message);
                return BadRequest("MainBoardController:ChangeCommissionRate hata:" + ex.Message);
            }
        }

    }
}

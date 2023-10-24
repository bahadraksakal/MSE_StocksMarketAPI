using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.Entities;
using StocksMarketWebAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StocksMarketWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;

        public AuthController(UserService userService, AuthService authService)
        {            
            _userService = userService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(string userName, string userPassword, string userEmail, string userTel)
        {
            try
            {
                User newUser = await _userService.AddUserAsync(userName, userPassword, userEmail, userTel, "User");
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                return new ContentResult
                {
                    Content = JsonSerializer.Serialize(newUser,options),
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

        [HttpPost]
        public async Task<IActionResult> Login(string userName,string userPassword)
        {
            try
            {
                User userTemp = await _authService.CheckCredentialsAsync(userName, userPassword);
                if (userTemp != null)
                {
                    string token = _authService.CreateToken(userTemp);

                    var response = new
                    {
                        User = userTemp,
                        Token = token
                    };
                    Log.Warning($"{userTemp.Id} Id'li kullanıcı olan {userTemp.Name} giriş yaptı. Tokeni: {token}");
                    return new ContentResult
                    {
                        Content = JsonSerializer.Serialize(response),
                        ContentType = "application/json",
                        StatusCode = 200 // Başarı durumu
                    };
                }
                Log.Warning($"{userName} hatalı giriş denemesi.");
                return BadRequest("Kullanıcı Adı Veya Şifre Hatalı");
            }
            catch(Exception ex)
            {
                Log.Error("AuthController : Login metodunda hata:" + ex.InnerException.Message);
                return StatusCode(500, "AuthController : Login metodunda hata!");
            }
            
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using StocksMarketWebAPI.DTOs.UserDTOs;
using StocksMarketWebAPI.Services;

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
        public async Task<IActionResult> Register([FromBody]UserDTO user)
        {
            try
            {
                UserDTO newUser = await _userService.AddUserAsync(user.Name, user.Password, user.Email, user.Tel, "User");
                return StatusCode(StatusCodes.Status201Created, newUser);
            }
            catch (Exception ex)
            {
                return BadRequest("AuthController-Register Hata:" + ex.InnerException?.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserDTO user)
        {
            try
            {
                UserDTO userTemp = await _authService.CheckCredentialsAsync(user.Name, user.Password);
                if (userTemp != null)
                {
                    string token = _authService.CreateToken(userTemp);

                    var response = new
                    {
                        User = userTemp,
                        Token = token
                    };
                    return Ok(response);
                }
                return BadRequest("Kullanıcı Adı Veya Şifre Hatalı");
            }
            catch(Exception ex)
            {
                return StatusCode(500, "AuthController : Login metodunda hata!" + ex.Message);
            }
            
        }
    }
}

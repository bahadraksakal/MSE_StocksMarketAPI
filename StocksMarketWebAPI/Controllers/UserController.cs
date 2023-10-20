using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.Entities;

namespace StocksMarketWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly StockMarketDbContext _stockMarketDbContext = new StockMarketDbContext();

        public UserController(ILogger<WeatherForecastController> logger)
        {
            this._logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(string userName, string userPassword, string userEmail, string userTel,string userRole)
        {
            PortfolioUser userPortfolio = new PortfolioUser
            {
                User = new User() { Name=userName,Password=userPassword,Email=userEmail,Tel=userTel,Role=userRole},
                Portfolio = new Portfolio() { Balance=0}
            };
            _stockMarketDbContext.PortfolioUser.Add(userPortfolio);
            await _stockMarketDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}

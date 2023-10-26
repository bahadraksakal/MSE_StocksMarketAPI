using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksMarketWebAPI.Context;
using StocksMarketWebAPI.DTOs.MainBoardDTOs;
using StocksMarketWebAPI.DTOs.StockPriceDTOs;
using StocksMarketWebAPI.Services;
using System.Security.Claims;

namespace StocksMarketWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController:ControllerBase
    {
        private readonly StockMarketDbContext _stockMarketDbContext;
        private readonly StockService _stockService;
        public StockController(StockMarketDbContext stockMarketDbContext, StockService stockService) 
        {
            _stockMarketDbContext = stockMarketDbContext;
            _stockService = stockService;
        }

        [HttpPost]
        public async Task<IActionResult> SetStocks([FromBody] List<StockPriceDTO> stockPriceDTOs)
        {
            try
            {
                await _stockService.SetStocks(stockPriceDTOs);
                return Ok("Hisse senetleri ve verileri başarıyla eklendi");
            }
            catch (Exception ex)
            {
                return BadRequest("StockController:SetStocks hata:" + ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStocks()
        {
            try
            {
                List<StockPriceDTO> stockPriceDTOs = await _stockService.GetStocks();
                return Ok(stockPriceDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest("StockController:GetStocks hata:" + ex.Message);
            }
        }


    }
}

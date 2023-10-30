using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksMarketWebAPI.DTOs.PortfolioStockDTOs;
using StocksMarketWebAPI.DTOs.StockPriceDTOs;
using StocksMarketWebAPI.Services;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace StocksMarketWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController:ControllerBase
    {
        private readonly StockService _stockService;
        public StockController(StockService stockService) 
        {
            _stockService = stockService;
        }

        [HttpPut("{buyingStockUnit}/{stockName}")]
        public async Task<IActionResult> BuyStock(int buyingStockUnit, string stockName)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("userId"));
                string info=await _stockService.BuyStockAsync(userId,buyingStockUnit,stockName);
                return Ok(info);
            }
            catch (Exception ex)
            {
                return BadRequest("StockController:SetStocks hata:" + ex.Message);
            }
        }

        [HttpPut("{sellingStockUnit}/{stockName}")]
        public async Task<IActionResult> SellStock(int sellingStockUnit, string stockName)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("userId"));
                string info = await _stockService.SellStockAsync(userId, sellingStockUnit, stockName);
                return Ok(info);
            }
            catch (Exception ex)
            {
                return BadRequest("StockController:SetStocks hata:" + ex.Message);
            }
        }

        [Authorize(Policy = "CustomServerPolicy")]
        [HttpPut]
        public async Task<IActionResult> SetStocks([FromBody] List<StockPriceDTO> stockPriceDTOs)
        {
            try
            {
                await _stockService.SetStocksAsync(stockPriceDTOs);
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
                List<StockPriceDTO> stockPriceDTOs = await _stockService.GetStocksAsync();
                return Ok(stockPriceDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest("StockController:GetStocks hata:" + ex.Message);
            }
        }

        [HttpGet("{stockName}")]
        public async Task<IActionResult> GetListStockPricesByName(string stockName)
        {
            try
            {
                List<StockPriceDTO> stockPriceDTOs = await _stockService.GetListStockPricesByNameAsync(stockName);
                return Ok(stockPriceDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest("StockController:GetStocks hata:" + ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOwnedStocks()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("userId"));
                List<PortfolioStockDTO> portfolioStockDTOs = await _stockService.GetOwnedStocksAsync(userId);
                return Ok(portfolioStockDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest("StockController:GetOwnedStocks hata:" + ex.Message);
            }
        }


    }
}

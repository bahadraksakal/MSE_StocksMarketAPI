using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModalsLibrary.DateModals;
using SharedServices.ExcelServices;
using System.Security.Claims;

namespace StocksMarketWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly ExportExcelService _exportExcelService;
        public ExcelController(ExportExcelService exportExcelService)
        {
            _exportExcelService = exportExcelService;
        }

        [HttpGet("{stockName}")]
        public async Task<IActionResult> ExportExcelStockPricesByName(string stockName)
        {
            try
            {
                string[] fileInfos = await _exportExcelService.StockPricesByNameExportToExcel(stockName);
                return PhysicalFile(fileInfos[0], "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileInfos[1], true);
            }
            catch (Exception ex)
            {
                return BadRequest("ExcelController:ExportExcelStockPricesByName: Hata:" + ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> ExportExcelUserStockBuyAndSale()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("userId"));
                string[] fileInfos = await _exportExcelService.StockBuyAndSaleActivityReportExportToExcel(userId);
                return PhysicalFile(fileInfos[0], "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileInfos[1], true);
            }
            catch (Exception ex)
            {
                return BadRequest("ExcelController:ExportExcelUserStockBuyAndSale: Hata:" + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportExcelUserStockBuyAndSaleByDate([FromBody] StartEndDateModal startEndDateModal)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("userId"));
                string[] fileInfos= await _exportExcelService.StockBuyAndSaleActivityByDateReportExportToExcel(userId,startEndDateModal.StartDate,startEndDateModal.EndDate);
                return PhysicalFile(fileInfos[0], "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileInfos[1], true);
            }
            catch (Exception ex)
            {
                return BadRequest("ExcelController:ExportExcelUserStockBuyAndSale: Hata:" + ex.Message);
            }
        }
    }
}

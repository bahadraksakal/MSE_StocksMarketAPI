using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModalsLibrary.DateModals;
using SharedServices.PdfServices;
using System.Security.Claims;

namespace StocksMarketWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly ExportPdfService _exportPdfService;
        public PdfController(ExportPdfService exportPdfService)
        {
            _exportPdfService = exportPdfService;
        }

        [HttpPost]
        public async Task<IActionResult> ExportPdfUserStockBuyAndSaleByDate([FromBody] StartEndDateModal startEndDateModal)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("userId"));
                string[] fileInfos = await _exportPdfService.StockBuyAndSaleActivityByDateReportExportToPdf(userId, startEndDateModal.StartDate, startEndDateModal.EndDate);
                return PhysicalFile(fileInfos[0], "application/pdf", fileInfos[1], true);
            }
            catch (Exception ex)
            {
                return BadRequest("PdfController:ExportExcelUserStockBuyAndSaleByDate: Hata:" + ex.Message);
            }
        }
    }
}

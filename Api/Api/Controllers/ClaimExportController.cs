using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClaimExportController : ControllerBase
    {
        private readonly IClaimExportService _exportService;

        public ClaimExportController(IClaimExportService exportService)
        {
            _exportService = exportService;
        }

        //Controlador para exportar las reclamaciones 
        [HttpGet("export")]
        public async Task<IActionResult> ExportClaims()
        {
            try
            {
                var fileBytes = await _exportService.ExportClaimsToCsvAsync();

                string fileName = $"claims_report_{DateTime.Now:yyyyMMdd_HHmm}.csv";

                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error during export: {ex.Message}");
            }
        }
    }
}

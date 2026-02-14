using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClaimImportsController : ControllerBase
    {
        private readonly IClaimImportService _importService;

        public ClaimImportsController(IClaimImportService importService)
        {
            _importService = importService;
        }

        //Controlador para importar archivo
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length ==0) 
                return BadRequest(new ApiResponse<string>(false, "Archivo no proporcionado o vacío."));

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (extension != ".csv")
                return BadRequest(new ApiResponse<string>(false, "Formato no permitido"));

            try
            {
                var result = await _importService.UploadAsync(file);
                return Ok(new ApiResponse<ClaimImportResponseDto>(true, "Archivo procesado exitosamente", result));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, $"Error al procesar el archivo: {ex.Message}"));
            }
        }
    }
}

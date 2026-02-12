using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimsService _claimsService;

        public ClaimsController(IClaimsService claimsService)
        {
            _claimsService = claimsService;
        }

        //Obtener el listado de reclamos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _claimsService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ClaimsDto>>(true, "Listado de reclamos obtenido con éxito", result));
        }

        //Registrar un nuevo reclamo
        [HttpPost]
        public async Task<IActionResult> Register(ClaimsDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new ApiResponse<string>(false, "El formato de los datos es inválido o faltan campos obligatorios."));
            }

            if (string.IsNullOrWhiteSpace(dto.patient_id.ToString()))
                return BadRequest(new ApiResponse<string>(false, "El id del paciente es obligatorio."));

            if (string.IsNullOrWhiteSpace(dto.claim_number))
                return BadRequest(new ApiResponse<string>(false, "El número de reclamo es obligatorio."));

            if (dto.service_date == default)
                return BadRequest(new ApiResponse<string>(false, "La fecha es obligatoría."));

            if (string.IsNullOrWhiteSpace(dto.amount.ToString()))
                return BadRequest(new ApiResponse<string>(false, "El monto es obligatorio."));

            if (string.IsNullOrWhiteSpace(dto.status.ToString()))
                return BadRequest(new ApiResponse<string>(false, "El status es obligatorio."));

            if (dto.created_at == default)
                return BadRequest(new ApiResponse<string>(false, "La fecha es obligatoría."));


            var result = await _claimsService.RegisterAsync(dto);

            return Ok(new ApiResponse<ClaimResponseDto>(true, "Paciente registrado con éxito", result));
        }
    }
}

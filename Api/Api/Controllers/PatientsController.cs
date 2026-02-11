using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }


        //Registrar nuevo paciente
        [HttpPost]
        public async Task<IActionResult> Register(PatientDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new ApiResponse<string>(false, "El formato de los datos es inválido o faltan campos obligatorios."));
            }

            if (string.IsNullOrWhiteSpace(dto.first_name))
                return BadRequest(new ApiResponse<string>(false, "El nombre es obligatorio."));

            if (string.IsNullOrWhiteSpace(dto.last_name))
                return BadRequest(new ApiResponse<string>(false, "El apellido es obligatorio."));

            if (dto.dob == default)
                return BadRequest(new ApiResponse<string>(false, "La fecha es obligatoría."));

            var result = await _patientService.RegisterAsync(dto);

            return Ok(new ApiResponse<PatientResponseDto>(true, "Paciente registrado con éxito", result));
        }

        //Obtener listado de pacientes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _patientService.GetAllAsync();

            return Ok(new ApiResponse<IEnumerable<PatientResponseDto>>(true, "Listado de pacientes obtenido", patients));
        }

        //Actualizar paciente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientDto dto)
        {

            if (dto == null)
                return BadRequest(new ApiResponse<string>(false, "Datos de actualización inválidos."));

            if (string.IsNullOrWhiteSpace(dto.first_name) || string.IsNullOrWhiteSpace(dto.last_name))
                return BadRequest(new ApiResponse<string>(false, "Nombre y apellido son requeridos."));

            if (dto.dob == default || dto.dob == null)
                return BadRequest(new ApiResponse<string>(false, "La fecha es obligatoria."));

            var result = await _patientService.UpdateAsync(id, dto);

            if (!result)
            {
                return NotFound(new ApiResponse<string>(false, $"No se encontró el paciente con ID {id} para actualizar."));
            }

            return Ok(new ApiResponse<string>(true, "Paciente actualizado con éxito."));
        }

        //Eliminar paciente
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _patientService.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new ApiResponse<string>(false, $"No se pudo eliminar: El paciente con ID {id} no existe."));
            }

            return Ok(new ApiResponse<string>(true, "Paciente eliminado correctamente."));
        }
    }
}

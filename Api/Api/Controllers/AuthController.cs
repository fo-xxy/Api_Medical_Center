using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    //Controlador usuarios 

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var validStatuses = new[] { "admin", "staff"};

            if (string.IsNullOrWhiteSpace(dto.email))
                return BadRequest(new ApiResponse<string>(false, "El correo electrónico es obligatorio."));

            if (string.IsNullOrWhiteSpace(dto.password_digest))
                return BadRequest(new ApiResponse<string>(false, "La contraseña es obligatorio."));

            if (string.IsNullOrWhiteSpace(dto.role))
                return BadRequest(new ApiResponse<string>(false, "El rol es obligatorio."));

            if (!validStatuses.Contains(dto.role.ToString()))
            {
                return BadRequest(new ApiResponse<string>(false,
                    $"Rol inválido. Los roles permitidos son: {string.Join(", ", validStatuses)}"));
            }

            var result = await _authService.RegisterAsync(dto);

            if (!result)
                return BadRequest("No se pudo completar el registro.");

            return Ok(new { message = "Usuario registrado con éxito" });
        }

  
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            if (token == null)
                return Unauthorized("Credenciales inválidas.");
            return Ok(new { token });
        }
    }
}


using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;

namespace Api.Controllers
{
        //Controlador usuarios 

        //Endpoint para registrar usuarios
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
                var result = await _authService.RegisterAsync(dto);

                if (!result)
                    return BadRequest("No se pudo completar el registro.");

                return Ok(new { message = "Usuario registrado con éxito" });
            }
        }
}


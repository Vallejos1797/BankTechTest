using Api.Contracts;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos de registro inválidos", errores = ModelState });

            try
            {
                var userId = await _authService.RegisterAsync(
                    request.Nombre.Trim(),
                    request.Email.Trim(),
                    request.Password,
                    request.Role?.Trim() ?? "user",
                    ct
                );

                var usuario = await _authService.GetByIdAsync(userId, ct);
                if (usuario is null)
                    return StatusCode(500, new { message = "No se pudo crear el usuario." });

                var token = _tokenService.CreateToken(
                    usuario,
                    new[] { new Claim(ClaimTypes.Role, usuario.Rol.Nombre) }
                );

                return CreatedAtAction(nameof(Register), new AuthResponse(
                    token,
                    new UserResponse(usuario.Id, usuario.Nombre, usuario.Email, usuario.Rol.Nombre)
                ));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error en registro");
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en registro");
                return StatusCode(500, new { message = "Ocurrió un error interno." });
            }
        }

        /// <summary>
        /// Autentica a un usuario y devuelve un JWT.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos de login inválidos", errores = ModelState });

            try
            {
                var usuario = await _authService.ValidateUserAsync(request.Nombre, request.Password, ct);
                if (usuario is null)
                {
                    _logger.LogWarning("Intento de login fallido para usuario {Nombre}", request.Nombre);
                    return Unauthorized(new { message = "Credenciales inválidas" });
                }

                var token = _tokenService.CreateToken(
                    usuario,
                    new[] { new Claim(ClaimTypes.Role, usuario.Rol.Nombre) }
                );

                return Ok(new AuthResponse(
                    token,
                    new UserResponse(usuario.Id, usuario.Nombre, usuario.Email, usuario.Rol.Nombre)
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en login");
                return StatusCode(500, new { message = "Ocurrió un error interno." });
            }
        }
    }
}

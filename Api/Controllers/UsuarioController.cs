using Application.DTOs.Usuario;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear(CreateUsuarioDto dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return Ok(new { Id = id, Message = "Usuario creado exitosamente" });
        }

        [HttpGet("todos")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var usuarios = await _service.GetAllAsync(ct);
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var usuario = await _service.GetByIdAsync(id, ct);
            if (usuario == null) return NotFound(new { Message = "Usuario no encontrado" });
            return Ok(usuario);
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> Update(UpdateUsuarioDto dto, CancellationToken ct)
        {
            await _service.UpdateAsync(dto, ct);
            return Ok(new { Message = "Usuario actualizado exitosamente" });
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _service.DeleteAsync(id, ct);
            return Ok(new { Message = "Usuario eliminado exitosamente" });
        }
    }
}
using Application.DTOs.Compra;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompraController : ControllerBase
    {
        private readonly CompraService _service;

        public CompraController(CompraService service)
        {
            _service = service;
        }

        /// <summary>
        /// Crear una nueva compra
        /// </summary>
        [HttpPost("crear")]
        public async Task<IActionResult> CrearCompra([FromBody] CreateCompraDto dto, CancellationToken ct)
        {
            var id = await _service.CreateAsync(dto, ct);
            return Ok(new { Id = id, Message = "Compra creada exitosamente" });
        }

        /// <summary>
        /// Obtener todas las compras
        /// </summary>
        [HttpGet("todas")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _service.GetAllAsync(ct);
            return Ok(result);
        }

        /// <summary>
        /// Obtener compras de un usuario específico
        /// </summary>
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId, CancellationToken ct)
        {
            var result = await _service.GetByUsuarioAsync(usuarioId, ct);
            return Ok(result);
        }

        /// <summary>
        /// Obtener detalle de una compra por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _service.GetByIdAsync(id, ct);
            if (result == null)
                return NotFound(new { Message = "Compra no encontrada" });

            return Ok(result);
        }
    }
}
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // solo usuarios autenticados
    public class ProveedoresController : ControllerBase
    {
        private readonly ProveedorService _service;

        public ProveedoresController(ProveedorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var proveedor = await _service.GetByIdAsync(id);
            if (proveedor == null) return NotFound();
            return Ok(proveedor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Proveedor proveedor)
        {
            var id = await _service.CreateAsync(proveedor);
            return CreatedAtAction(nameof(GetById), new { id }, proveedor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Proveedor proveedor)
        {
            if (id != proveedor.Id) return BadRequest();
            await _service.UpdateAsync(proveedor);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
using Application.DTOs.ProductoProveedor;
using Application.Ports;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoProveedorController : ControllerBase
    {
        private readonly IProductoProveedorRepository _repo;

        public ProductoProveedorController(IProductoProveedorRepository repo)
        {
            _repo = repo;
        }

        // POST: api/ProductoProveedor
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductoProveedorDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newId = await _repo.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetByProducto), new { productoId = dto.ProductoId }, dto);
        }

        // GET: api/ProductoProveedor/ByProducto/5
        [HttpGet("ByProducto/{productoId}")]
        public async Task<IActionResult> GetByProducto(int productoId, CancellationToken ct)
        {
            var relaciones = await _repo.GetByProductoIdAsync(productoId, ct);
            return Ok(relaciones);
        }

        // DELETE: api/ProductoProveedor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _repo.DeleteAsync(id, ct);
            return NoContent();
        }
        
        // GET: api/ProductoProveedor
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var relaciones = await _repo.GetAllAsync(ct);
            return Ok(relaciones);
        }
        
        // PUT: api/ProductoProveedor
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProductoProveedorDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repo.UpdateAsync(dto, ct);
            return NoContent();
        }

    }
}
using Application.DTOs.ProductoProveedor;

namespace Application.Ports
{
    public interface IProductoProveedorRepository
    {
        Task<int> CreateAsync(CreateProductoProveedorDto dto, CancellationToken ct);
        Task<List<ProductoProveedorResponseDto>> GetAllAsync(CancellationToken ct);
        Task<List<ProductoProveedorResponseDto>> GetByProductoIdAsync(int productoId, CancellationToken ct);
        Task UpdateAsync(UpdateProductoProveedorDto dto, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
    }
}
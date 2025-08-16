using Domain.Entities;

namespace Application.Ports
{
    public interface IProductRepository
    {
        Task<List<Producto>> GetAllAsync(CancellationToken ct);
        Task<Producto?> GetByIdAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(Producto producto, CancellationToken ct);
        Task UpdateAsync(Producto producto, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
    }
}
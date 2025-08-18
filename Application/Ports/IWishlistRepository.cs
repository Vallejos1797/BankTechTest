using Domain.Entities;

namespace Application.Ports
{
    public interface IWishlistRepository
    {
        Task<IEnumerable<Producto>> GetByUserAsync(int userId, CancellationToken ct);
        Task<bool> AddAsync(int userId, int productId, CancellationToken ct);
        Task RemoveAsync(int userId, int productId, CancellationToken ct);
    }
}
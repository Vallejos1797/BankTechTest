using Application.Ports;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly AppDbContext _context;

        public WishlistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetByUserAsync(int userId, CancellationToken ct)
        {
            return await _context.Wishlists
                .Where(w => w.UsuarioId == userId)
                .Include(w => w.Producto)
                .Select(w => w.Producto)
                .ToListAsync(ct);
        }

        public async Task<bool> AddAsync(int userId, int productId, CancellationToken ct)
        {
            var exists = await _context.Wishlists
                .AnyAsync(w => w.UsuarioId == userId && w.ProductoId == productId, ct);

            if (exists) return false;

            var wishlistItem = new Wishlist
            {
                UsuarioId = userId,
                ProductoId = productId
            };

            _context.Wishlists.Add(wishlistItem);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task RemoveAsync(int userId, int productId, CancellationToken ct)
        {
            var item = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UsuarioId == userId && w.ProductoId == productId, ct);

            if (item != null)
            {
                _context.Wishlists.Remove(item);
                await _context.SaveChangesAsync(ct);
            }
        }
    }
}
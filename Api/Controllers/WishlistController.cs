using Application.Ports;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Comprador")] // Solo compradores pueden acceder
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistRepository _wishlistRepository;

        public WishlistController(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        // GET: api/Wishlist
        [HttpGet]
        public async Task<IActionResult> GetMyWishlist(CancellationToken ct)
        {
            var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine("🔍 Valor del Claim NameIdentifier: " + claimValue);

            try
            {
                var userId = int.Parse(claimValue ?? string.Empty);
                Console.WriteLine("✅ userId convertido a int: " + userId);

                var wishlist = await _wishlistRepository.GetByUserAsync(userId, ct);
                Console.WriteLine("📦 Wishlist obtenida: " + (wishlist != null ? "Sí" : "No"));

                return Ok(wishlist);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("❌ Error al convertir Claim a int: " + ex.Message);
                return BadRequest(new { error = "El identificador del usuario no es válido", claimValue });
            }
        }


        // POST: api/Wishlist/{productId}
        [HttpPost("{productId}")]
        public async Task<IActionResult> AddToWishlist(int productId, CancellationToken ct)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var added = await _wishlistRepository.AddAsync(userId, productId, ct);

            if (!added)
                return BadRequest(new { message = "El producto ya está en tu lista de deseados" });

            return Ok(new { message = "Producto agregado a la lista de deseados" });
        }

        // DELETE: api/Wishlist/{productId}
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromWishlist(int productId, CancellationToken ct)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            await _wishlistRepository.RemoveAsync(userId, productId, ct);

            return Ok(new { message = "Producto eliminado de la lista de deseados" });
        }
    }
}
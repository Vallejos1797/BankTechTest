using Application.Ports;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByNombreOrEmailAsync(string nombre, string email, CancellationToken ct)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Nombre == nombre || u.Email == email, ct);
        }

        public async Task<Usuario?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task<Usuario?> GetByNombreAsync(string nombre, CancellationToken ct)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Nombre == nombre, ct);
        }

        public async Task AddAsync(Usuario usuario, CancellationToken ct)
        {
            await _context.Usuarios.AddAsync(usuario, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<Rol?> GetRolByNombreAsync(string nombreRol, CancellationToken ct)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == nombreRol, ct);
        }
    }
}
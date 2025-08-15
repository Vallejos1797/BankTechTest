using Domain.Entities;

namespace Application.Ports
{
    public interface IUsuarioRepository
    {
        Task<bool> ExistsByNombreOrEmailAsync(string nombre, string email, CancellationToken ct);
        Task<Usuario?> GetByIdAsync(int id, CancellationToken ct);
        Task<Usuario?> GetByNombreAsync(string nombre, CancellationToken ct);
        Task AddAsync(Usuario usuario, CancellationToken ct);
        
        // Nuevo: obtener rol por nombre
        Task<Rol?> GetRolByNombreAsync(string nombreRol, CancellationToken ct);
    }
}
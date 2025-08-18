using Application.DTOs.Usuario;
using Domain.Entities;

namespace Application.Ports
{
    public interface IUsuarioRepository
    {
        // Verificar si existe un usuario por nombre o email
        Task<bool> ExistsByNombreOrEmailAsync(string nombre, string email, CancellationToken ct);

        // Obtener usuario por ID
        Task<UsuarioResponseDto?> GetByIdAsync(int id, CancellationToken ct);

        // Obtener usuario por nombre (para login, validaciones, etc.)
        Task<Usuario?> GetByNombreAsync(string nombre, CancellationToken ct);

        // Crear usuario
        Task<int> CreateAsync(CreateUsuarioDto dto, CancellationToken ct);

        // Actualizar usuario
        Task UpdateAsync(UpdateUsuarioDto dto, CancellationToken ct);

        // Eliminar usuario
        Task DeleteAsync(int id, CancellationToken ct);

        // Obtener todos los usuarios
        Task<List<UsuarioResponseDto>> GetAllAsync(CancellationToken ct);

        // Obtener rol por nombre (lo que ya tienes)
        Task<Rol?> GetRolByNombreAsync(string nombreRol, CancellationToken ct);
    }
}
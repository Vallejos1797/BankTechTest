using Application.DTOs.Usuario;
using Application.Ports;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _db;

        public UsuarioRepository(AppDbContext db)
        {
            _db = db;
        }

        // Verificar si existe un usuario por nombre o email
        public async Task<bool> ExistsByNombreOrEmailAsync(string nombre, string email, CancellationToken ct)
        {
            return await _db.Usuarios.AnyAsync(
                u => u.Nombre == nombre || u.Email == email, ct);
        }

        // Obtener todos los usuarios (SP)
        public Task<List<UsuarioResponseDto>> GetAllAsync(CancellationToken ct) =>
            _db.Database
               .SqlQuery<UsuarioResponseDto>($"EXEC usp_Usuarios_GetAll")
               .ToListAsync(ct);

        // Obtener usuario por Id (SP)
        public async Task<UsuarioResponseDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var result = await _db.Database
                .SqlQuery<UsuarioResponseDto>($"EXEC usp_Usuarios_GetById @Id={id}")
                .ToListAsync(ct);

            return result.FirstOrDefault();
        }

        // Obtener usuario por nombre (para login)
        public async Task<Usuario?> GetByNombreAsync(string nombre, CancellationToken ct)
        {
            return await _db.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Nombre == nombre, ct);
        }

        // Crear usuario (SP)
        public async Task<int> CreateAsync(CreateUsuarioDto dto, CancellationToken ct)
        {
            var result = await _db.Database
                .SqlQuery<int>($@"
                    EXEC usp_Usuarios_Create
                        @Nombre={dto.Nombre},
                        @Email={dto.Email},
                        @PasswordHash={dto.PasswordHash},
                        @RolId={dto.RolId}
                ")
                .ToListAsync(ct);

            return result.Single();
        }

        // Actualizar usuario (SP)
        public Task UpdateAsync(UpdateUsuarioDto dto, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(dto.PasswordHash))
            {
                return _db.Database.ExecuteSqlInterpolatedAsync($@"
            EXEC usp_Usuarios_Update
                @Id={dto.Id},
                @Nombre={dto.Nombre},
                @Email={dto.Email},
                @RolId={dto.RolId}
        ", ct);
            }
            else
            {
                return _db.Database.ExecuteSqlInterpolatedAsync($@"
            EXEC usp_Usuarios_Update
                @Id={dto.Id},
                @Nombre={dto.Nombre},
                @Email={dto.Email},
                @PasswordHash={dto.PasswordHash},
                @RolId={dto.RolId}
        ", ct);
            }
        }

        // Eliminar usuario (SP)
        public Task DeleteAsync(int id, CancellationToken ct) =>
            _db.Database.ExecuteSqlInterpolatedAsync($@"EXEC usp_Usuarios_Delete @Id={id}", ct);

        // Obtener rol por nombre (Entity)
        public async Task<Rol?> GetRolByNombreAsync(string nombreRol, CancellationToken ct)
        {
            return await _db.Roles.FirstOrDefaultAsync(r => r.Nombre == nombreRol, ct);
        }
    }
}

using Application.Services;
using Domain.Entities;
using System.Security.Cryptography;
using System.Text;
using Application.Ports;
using Application.DTOs.Usuario;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AuthService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<int> RegisterAsync(string nombre, string email, string password, string rolNombre, CancellationToken ct)
    {
        if (await _usuarioRepository.ExistsByNombreOrEmailAsync(nombre, email, ct))
            throw new InvalidOperationException("El usuario o email ya están registrados.");

        var passwordHash = HashPassword(password);

        // Buscar rol por nombre
        var rol = await _usuarioRepository.GetRolByNombreAsync(rolNombre, ct);
        if (rol is null)
            throw new InvalidOperationException($"El rol '{rolNombre}' no existe.");

        // Usamos DTO porque así está definido el repo
        var dto = new CreateUsuarioDto
        {
            Nombre = nombre,
            Email = email,
            PasswordHash = passwordHash,
            RolId = rol.Id
        };

        var id = await _usuarioRepository.CreateAsync(dto, ct);

        return id;
    }

    public async Task<Usuario?> GetByIdAsync(int id, CancellationToken ct)
    {
        var dto = await _usuarioRepository.GetByIdAsync(id, ct);
        if (dto == null) return null;

        // Convertimos el DTO a entidad de dominio para mantener compatibilidad
        return new Usuario
        {
            Id = dto.Id,
            Nombre = dto.Nombre,
            Email = dto.Email,
            PasswordHash = "****", // no lo devuelve el SP
            RolId = dto.RolId,
            Rol = new Rol { Id = dto.RolId, Nombre = dto.RolNombre },
            FechaCreacion = dto.FechaCreacion
        };
    }

    public async Task<Usuario?> ValidateUserAsync(string nombre, string password, CancellationToken ct)
    {
        var usuario = await _usuarioRepository.GetByNombreAsync(nombre, ct);
        if (usuario is null) return null;

        return VerifyPassword(password, usuario.PasswordHash) ? usuario : null;
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private static bool VerifyPassword(string password, string storedHash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == storedHash;
    }
}

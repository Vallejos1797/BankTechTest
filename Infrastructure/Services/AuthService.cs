using Application.Services;
using Domain.Entities;
using System.Security.Cryptography;
using System.Text;
using Application.Ports;

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

        var usuario = new Usuario
        {
            Nombre = nombre,
            Email = email,
            PasswordHash = passwordHash,
            RolId = rol.Id, // Asignar FK
            Rol = rol       // Asignar entidad
        };

        await _usuarioRepository.AddAsync(usuario, ct);

        return usuario.Id;
    }


    public async Task<Usuario?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _usuarioRepository.GetByIdAsync(id, ct);
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
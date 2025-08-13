using Application.Ports;
using Application.Services.Security;
using Domain.Entities;

namespace Application.Services;

public class AuthService
{
    private readonly IUserRepository _users;
    public AuthService(IUserRepository users) => _users = users;

    public async Task<int> RegisterAsync(string username, string email, string password, string role, CancellationToken ct)
    {
        var existing = await _users.GetByUsernameAsync(username, ct);
        if (existing is not null) throw new InvalidOperationException("Username ya existe");

        var (hash, salt) = PasswordHasher.Hash(password);
        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = string.IsNullOrWhiteSpace(role) ? "user" : role,
            CreatedAt = DateTime.UtcNow
        };
        return await _users.CreateAsync(user, ct);
    }

    public async Task<User?> ValidateUserAsync(string username, string password, CancellationToken ct)
    {
        var user = await _users.GetByUsernameAsync(username, ct);
        if (user is null) return null;
        return PasswordHasher.Verify(password, user.PasswordHash, user.PasswordSalt) ? user : null;
    }

    public Task<List<User>> GetAllAsync(CancellationToken ct) => _users.GetAllAsync(ct);
    public Task<User?> GetByIdAsync(int id, CancellationToken ct) => _users.GetByIdAsync(id, ct);

    public async Task UpdateAsync(int id, string email, string? newPassword, string role, CancellationToken ct)
    {
        var u = await _users.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("No existe");
        u.Email = email;
        u.Role = string.IsNullOrWhiteSpace(role) ? u.Role : role;
        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            var (hash, salt) = PasswordHasher.Hash(newPassword);
            u.PasswordHash = hash; u.PasswordSalt = salt;
        }
        await _users.UpdateAsync(u, ct);
    }

    public Task DeleteAsync(int id, CancellationToken ct) => _users.DeleteAsync(id, ct);
}

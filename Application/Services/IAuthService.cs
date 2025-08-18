using Domain.Entities;

namespace Application.Services;

public interface IAuthService
{
    Task<int> RegisterAsync(string nombre, string email, string password, string rol, CancellationToken ct);
    Task<Usuario?> GetByIdAsync(int id, CancellationToken ct);
    Task<Usuario?> ValidateUserAsync(string nombre, string password, CancellationToken ct);
}
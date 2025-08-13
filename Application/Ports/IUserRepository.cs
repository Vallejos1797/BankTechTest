using Domain.Entities;

namespace Application.Ports;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id, CancellationToken ct);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct);
    Task<List<User>> GetAllAsync(CancellationToken ct);
    Task<int> CreateAsync(User user, CancellationToken ct);
    Task UpdateAsync(User user, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}
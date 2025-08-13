using Application.Ports;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;    
namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public async Task<int> CreateAsync(User u, CancellationToken ct)
    {
        var ids = await _db.Database
            .SqlQuery<int>($@"
            EXEC dbo.usp_User_Create
                @Username={u.Username}, @Email={u.Email},
                @PasswordHash={u.PasswordHash}, @PasswordSalt={u.PasswordSalt},
                @Role={u.Role}")
            .ToListAsync(ct);     // ejecuta el SQL en servidor

        var newId = ids.Single(); // compone en memoria (lado cliente)
        return newId;
    }

    public Task DeleteAsync(int id, CancellationToken ct) =>
        _db.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.usp_User_Delete @Id={id}", ct);

    public async Task<List<User>> GetAllAsync(CancellationToken ct) =>
        await _db.Users.FromSqlRaw("EXEC dbo.usp_User_GetAll").AsNoTracking().ToListAsync(ct);

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct)
    {
        var rows = await _db.Users
            .FromSqlInterpolated($"EXEC dbo.usp_User_GetById @Id={id}")
            .AsNoTracking().ToListAsync(ct);
        return rows.FirstOrDefault();
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct)
    {
        var rows = await _db.Users
            .FromSqlInterpolated($"EXEC dbo.usp_User_GetByUsername @Username={username}")
            .AsNoTracking().ToListAsync(ct);
        return rows.FirstOrDefault();
    }

    public Task UpdateAsync(User u, CancellationToken ct) =>
        _db.Database.ExecuteSqlInterpolatedAsync($@"
            EXEC dbo.usp_User_Update
                @Id={u.Id}, @Email={u.Email},
                @PasswordHash={u.PasswordHash}, @PasswordSalt={u.PasswordSalt},
                @Role={u.Role}", ct);
}
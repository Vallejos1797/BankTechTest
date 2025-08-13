using Application.Ports;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;
    public ProductRepository(AppDbContext db) => _db = db;

    public Task<List<Product>> GetAllAsync(CancellationToken ct) =>
        _db.Products.FromSqlRaw("EXEC dbo.usp_Product_GetAll")
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Product?> GetByIdAsync(int id, CancellationToken ct)
    {
        var rows = await _db.Products
            .FromSqlInterpolated($"EXEC dbo.usp_Product_GetById @Id={id}")
            .AsNoTracking()
            .ToListAsync(ct);
        return rows.FirstOrDefault();
    }

    public async Task<int> CreateAsync(Product p, CancellationToken ct)
    {
        var ids = await _db.Database
            .SqlQuery<int>($@"
            EXEC dbo.usp_Product_Create
                @Name={p.Name},
                @Description={p.Description},
                @Logo={p.Logo},
                @DateRelease={p.DateRelease},
                @DateRevision={p.DateRevision}")
            .ToListAsync(ct);        // ⬅️ ejecuta el SQL
        return ids.Single();         // ⬅️ composición en memoria (cliente)
    }


    public Task UpdateAsync(Product p, CancellationToken ct) =>
        _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC dbo.usp_Product_Update @Id={p.Id}, @Name={p.Name}, @Description={p.Description}, @Logo={p.Logo}, @DateRelease={p.DateRelease}, @DateRevision={p.DateRevision}", ct);

    public Task DeleteAsync(int id, CancellationToken ct) =>
        _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC dbo.usp_Product_Delete @Id={id}", ct);
}
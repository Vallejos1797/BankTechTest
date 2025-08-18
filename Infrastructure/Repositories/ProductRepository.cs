using Application.Ports;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<List<Producto>> GetAllAsync(CancellationToken ct) =>
            _db.Productos
                .FromSqlRaw("EXEC usp_Product_GetAll")
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<Producto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var result = await _db.Productos
                .FromSqlInterpolated($"EXEC usp_Product_GetById @Id={id}")
                .AsNoTracking()
                .ToListAsync(ct);

            return result.FirstOrDefault();
        }

        public async Task<int> CreateAsync(Producto producto, CancellationToken ct)
        {
            var result = await _db.Database
                .SqlQuery<int>($@"
                    EXEC usp_Product_Create
                        @Nombre={producto.Nombre},
                        @Descripcion={producto.Descripcion},
                        @ImagenUrl={producto.ImagenUrl}
                ")
                .ToListAsync(ct);

            return result.Single();
        }

        public Task UpdateAsync(Producto producto, CancellationToken ct) =>
            _db.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC usp_Product_Update
                    @Id={producto.Id},
                    @Nombre={producto.Nombre},
                    @Descripcion={producto.Descripcion},
                    @ImagenUrl={producto.ImagenUrl}
            ", ct);

        public Task DeleteAsync(int id, CancellationToken ct) =>
            _db.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC usp_Product_Delete @Id={id}
            ", ct);
    }
}
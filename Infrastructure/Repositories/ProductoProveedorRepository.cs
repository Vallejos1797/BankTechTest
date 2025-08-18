using Application.DTOs.ProductoProveedor;
using Application.Ports;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductoProveedorRepository : IProductoProveedorRepository
    {
        private readonly AppDbContext _db;

        public ProductoProveedorRepository(AppDbContext db)
        {
            _db = db;
        }

        // Crear relación producto–proveedor
        public async Task<int> CreateAsync(CreateProductoProveedorDto dto, CancellationToken ct)
        {
            var result = await _db.Database
                .SqlQuery<int>($@"
                    EXEC usp_ProductoProveedor_Create
                        @ProductoId={dto.ProductoId},
                        @ProveedorId={dto.ProveedorId},
                        @Precio={dto.Precio},
                        @Stock={dto.Stock},
                        @Lote={dto.Lote}
                ")
                .ToListAsync(ct);

            if (!result.Any())
                throw new Exception("Error al crear la relación Producto–Proveedor.");

            return result.Single();
        }

        // Obtener todas las relaciones (con nombres)
        public Task<List<ProductoProveedorResponseDto>> GetAllAsync(CancellationToken ct) =>
            _db.Database
                .SqlQuery<ProductoProveedorResponseDto>($"EXEC usp_ProductoProveedor_GetAll")
                .ToListAsync(ct);

        // Obtener relaciones por producto específico
        public Task<List<ProductoProveedorResponseDto>> GetByProductoIdAsync(int productoId, CancellationToken ct) =>
            _db.Database
                .SqlQuery<ProductoProveedorResponseDto>(
                    $"EXEC usp_ProductoProveedor_GetByProductoId @ProductoId={productoId}")
                .ToListAsync(ct);

        // Eliminar relación
        public Task DeleteAsync(int id, CancellationToken ct) =>
            _db.Database.ExecuteSqlInterpolatedAsync($@"EXEC usp_ProductoProveedor_Delete @Id={id}", ct);

        public Task UpdateAsync(UpdateProductoProveedorDto dto, CancellationToken ct) =>
            _db.Database.ExecuteSqlInterpolatedAsync($@"
        EXEC usp_ProductoProveedor_Update
            @Id={dto.Id},
            @Precio={dto.Precio},
            @Stock={dto.Stock},
            @Lote={dto.Lote}
    ", ct);
    }
}
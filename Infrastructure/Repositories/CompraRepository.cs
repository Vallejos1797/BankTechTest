using Application.DTOs.Compra;
using Application.Ports;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CompraRepository : ICompraRepository
    {
        private readonly AppDbContext _db;

        public CompraRepository(AppDbContext db)
        {
            _db = db;
        }

        // Crear compra
        public async Task<int> CreateAsync(CreateCompraDto dto, CancellationToken ct)
        {
            var result = await _db.Database
                .SqlQuery<int>($@"
                    EXEC usp_Compras_Create
                        @UsuarioId={dto.UsuarioId},
                        @ProductoId={dto.ProductoId},
                        @PrecioCompra={dto.PrecioCompra},
                        @Cantidad={dto.Cantidad}
                ")
                .ToListAsync(ct);

            if (!result.Any())
                throw new Exception("Error al crear la compra.");

            return result.Single();
        }

        // Obtener todas las compras
        public Task<List<CompraResponseDto>> GetAllAsync(CancellationToken ct) =>
            _db.Database
                .SqlQuery<CompraResponseDto>($"EXEC usp_Compras_GetAll")
                .ToListAsync(ct);

        // Obtener compras por usuario
        public Task<List<CompraResponseDto>> GetByUsuarioAsync(int usuarioId, CancellationToken ct) =>
            _db.Database
                .SqlQuery<CompraResponseDto>($"EXEC usp_Compras_GetByUsuario @UsuarioId={usuarioId}")
                .ToListAsync(ct);

        // Obtener detalle de compra por Id
        public async Task<CompraResponseDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var result = await _db.Database
                .SqlQuery<CompraResponseDto>($"EXEC usp_Compras_GetById @Id={id}")
                .ToListAsync(ct);

            return result.FirstOrDefault();
        }
    }
}
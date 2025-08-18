using Application.Ports;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly AppDbContext _context;

        public ProveedorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proveedor>> GetAllAsync()
        {
            return await _context.Proveedores
                .FromSqlRaw("EXEC usp_Proveedor_GetAll")
                .ToListAsync();
        }

        public async Task<Proveedor?> GetByIdAsync(int id)
        {
            var result = await _context.Proveedores
                .FromSqlRaw("EXEC usp_Proveedor_GetById @Id",
                    new SqlParameter("@Id", id))
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<int> CreateAsync(Proveedor proveedor)
        {
            var id = await _context.Database.ExecuteSqlRawAsync(
                "EXEC usp_Proveedor_Create @Nombre, @Contacto, @Telefono",
                new SqlParameter("@Nombre", proveedor.Nombre),
                new SqlParameter("@Contacto", (object?)proveedor.Contacto ?? DBNull.Value),
                new SqlParameter("@Telefono", (object?)proveedor.Telefono ?? DBNull.Value)
            );

            return id;
        }

        public async Task UpdateAsync(Proveedor proveedor)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC usp_Proveedor_Update @Id, @Nombre, @Contacto, @Telefono",
                new SqlParameter("@Id", proveedor.Id),
                new SqlParameter("@Nombre", proveedor.Nombre),
                new SqlParameter("@Contacto", (object?)proveedor.Contacto ?? DBNull.Value),
                new SqlParameter("@Telefono", (object?)proveedor.Telefono ?? DBNull.Value)
            );
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC usp_Proveedor_Delete @Id",
                new SqlParameter("@Id", id)
            );
        }
    }
}

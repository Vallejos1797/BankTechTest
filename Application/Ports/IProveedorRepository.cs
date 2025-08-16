using Domain.Entities;

namespace Application.Ports
{
    public interface IProveedorRepository
    {
        Task<IEnumerable<Proveedor>> GetAllAsync();
        Task<Proveedor?> GetByIdAsync(int id);
        Task<int> CreateAsync(Proveedor proveedor);
        Task UpdateAsync(Proveedor proveedor);
        Task DeleteAsync(int id);
    }
}
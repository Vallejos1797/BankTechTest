using Application.Ports;
using Domain.Entities;

namespace Application.Services;

public class ProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public Task<List<Product>> GetAllAsync(CancellationToken ct) =>
        _repo.GetAllAsync(ct);

    public Task<Product?> GetByIdAsync(int id, CancellationToken ct) =>
        _repo.GetByIdAsync(id, ct);

    public async Task<int> CreateAsync(Product p, CancellationToken ct)
    {
        // Ejemplo de validaciones de negocio
        if (string.IsNullOrWhiteSpace(p.Name))
            throw new ArgumentException("El nombre es obligatorio");

        if (p.DateRevision < p.DateRelease)
            throw new ArgumentException("La fecha de revisión no puede ser anterior a la fecha de lanzamiento");

        return await _repo.CreateAsync(p, ct);
    }

    public Task UpdateAsync(Product p, CancellationToken ct) =>
        _repo.UpdateAsync(p, ct);

    public Task DeleteAsync(int id, CancellationToken ct) =>
        _repo.DeleteAsync(id, ct);
}
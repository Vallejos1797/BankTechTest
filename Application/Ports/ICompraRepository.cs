using Application.DTOs.Compra;

namespace Application.Ports
{
    public interface ICompraRepository
    {
        Task<int> CreateAsync(CreateCompraDto dto, CancellationToken ct);
        Task<List<CompraResponseDto>> GetAllAsync(CancellationToken ct);
        Task<List<CompraResponseDto>> GetByUsuarioAsync(int usuarioId, CancellationToken ct);
        Task<CompraResponseDto?> GetByIdAsync(int id, CancellationToken ct);
    }
}
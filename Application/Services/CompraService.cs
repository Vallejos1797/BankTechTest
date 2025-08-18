using Application.DTOs.Compra;
using Application.Ports;

namespace Application.Services
{
    public class CompraService
    {
        private readonly ICompraRepository _repo;

        public CompraService(ICompraRepository repo)
        {
            _repo = repo;
        }

        // Crear compra
        public async Task<int> CreateAsync(CreateCompraDto dto, CancellationToken ct)
        {
            return await _repo.CreateAsync(dto, ct);
        }

        // Obtener todas las compras
        public async Task<List<CompraResponseDto>> GetAllAsync(CancellationToken ct)
        {
            return await _repo.GetAllAsync(ct);
        }

        // Obtener compras por usuario
        public async Task<List<CompraResponseDto>> GetByUsuarioAsync(int usuarioId, CancellationToken ct)
        {
            return await _repo.GetByUsuarioAsync(usuarioId, ct);
        }

        // Obtener compra por Id
        public async Task<CompraResponseDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _repo.GetByIdAsync(id, ct);
        }
    }
}
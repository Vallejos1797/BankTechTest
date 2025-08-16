using Application.DTOs.Usuario;
using Application.Ports;

namespace Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        public Task<int> CreateAsync(CreateUsuarioDto dto, CancellationToken ct) =>
            _repo.CreateAsync(dto, ct);

        public Task<List<UsuarioResponseDto>> GetAllAsync(CancellationToken ct) =>
            _repo.GetAllAsync(ct);

        public Task<UsuarioResponseDto?> GetByIdAsync(int id, CancellationToken ct) =>
            _repo.GetByIdAsync(id, ct);

        public Task UpdateAsync(UpdateUsuarioDto dto, CancellationToken ct) =>
            _repo.UpdateAsync(dto, ct);

        public Task DeleteAsync(int id, CancellationToken ct) =>
            _repo.DeleteAsync(id, ct);
    }
}
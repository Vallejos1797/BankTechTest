namespace Application.DTOs.Usuario
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int RolId { get; set; }
        public string RolNombre { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
    }
}
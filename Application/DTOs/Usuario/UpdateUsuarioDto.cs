namespace Application.DTOs.Usuario
{
    public class UpdateUsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PasswordHash { get; set; } = null!;
        public int RolId { get; set; }
    }
}
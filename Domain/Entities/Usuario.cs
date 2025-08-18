namespace Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public int RolId { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Navegación
        public Rol Rol { get; set; } = null!;
    }
}
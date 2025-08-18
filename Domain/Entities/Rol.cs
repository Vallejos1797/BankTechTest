namespace Domain.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        // Relación con Usuarios
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
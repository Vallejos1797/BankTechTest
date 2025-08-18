namespace Domain.Entities
{
    public class ListaDeseo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ProductoId { get; set; }
        public DateTime FechaAgregado { get; set; }

        // Navegación
        public Usuario Usuario { get; set; } = null!;
        public Producto Producto { get; set; } = null!;
    }
}
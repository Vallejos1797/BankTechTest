using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Wishlist")] // 👈 fuerza a usar el nombre correcto de la tabla
    public class Wishlist
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ProductoId { get; set; }
        public DateTime FechaAgregado { get; set; }

        public Usuario Usuario { get; set; }
        public Producto Producto { get; set; }
    }
}
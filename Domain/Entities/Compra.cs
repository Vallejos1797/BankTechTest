namespace Domain.Entities;

public class Compra
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int ProductoId { get; set; }
    public decimal PrecioCompra { get; set; }
    public int Cantidad { get; set; }
    public DateTime FechaCompra { get; set; }
}
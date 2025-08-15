namespace Domain.Entities;

public class ProductoProveedor
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public int ProveedorId { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string? Lote { get; set; }
}
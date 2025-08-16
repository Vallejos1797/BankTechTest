namespace Application.DTOs.ProductoProveedor
{
    public class CreateProductoProveedorDto
    {
        public int ProductoId { get; set; }
        public int ProveedorId { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Lote { get; set; } = string.Empty;
    }
}
namespace Application.DTOs.ProductoProveedor
{
    public class UpdateProductoProveedorDto
    {
        public int Id { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Lote { get; set; } = string.Empty;
    }
}
namespace Application.DTOs.ProductoProveedor
{
    public class ProductoProveedorResponseDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty; // ✅ Nueva propiedad
        public int ProveedorId { get; set; }
        public string ProveedorNombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Lote { get; set; } = string.Empty;
    }
}
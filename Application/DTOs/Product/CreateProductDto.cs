namespace Application.DTOs.Product
{
    public class CreateProductDto
    {
        // Datos básicos del producto
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Logo { get; set; } = null!;
        public DateTime FechaLanzamiento { get; set; }
        public DateTime FechaRevision { get; set; }

        // Datos de proveedor (para ProductoProveedor)
        public int ProveedorId { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Lote { get; set; } = string.Empty;
    }
}
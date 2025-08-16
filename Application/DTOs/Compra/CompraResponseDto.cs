namespace Application.DTOs.Compra
{
    public class CompraResponseDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public decimal PrecioCompra { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaCompra { get; set; }
    }
}
namespace Domain.Entities
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Contacto { get; set; }
        public string? Telefono { get; set; }
    }
}
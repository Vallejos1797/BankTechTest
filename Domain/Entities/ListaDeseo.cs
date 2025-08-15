namespace Domain.Entities;

public class ListaDeseo
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int ProductoId { get; set; }
    public DateTime FechaAgregado { get; set; }
}
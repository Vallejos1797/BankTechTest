namespace Domain.Entities;

public class Product
{
    public int Id { get; set; }                // PK Identity
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Logo { get; set; } = null!;  // URL o path
    public DateTime DateRelease { get; set; }
    public DateTime DateRevision { get; set; }
}
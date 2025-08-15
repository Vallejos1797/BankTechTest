using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Proveedor> Proveedores { get; set; }
    public DbSet<ProductoProveedor> ProductosProveedores { get; set; }
    public DbSet<ListaDeseo> ListasDeseos { get; set; }
    public DbSet<Compra> Compras { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Usuarios
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
            entity.Property(u => u.FechaCreacion).HasDefaultValueSql("GETDATE()");
            entity.HasOne(u => u.Rol)
                  .WithMany(r => r.Usuarios)
                  .HasForeignKey(u => u.RolId);
        });

        // Roles
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Nombre).IsRequired().HasMaxLength(50);
        });

        // Productos
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Productos");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(p => p.Descripcion).HasColumnType("NVARCHAR(MAX)");
            entity.Property(p => p.ImagenUrl).HasMaxLength(255);
            entity.Property(p => p.FechaCreacion).HasDefaultValueSql("GETDATE()");
        });

        // Proveedores
        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.ToTable("Proveedores");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(p => p.Contacto).HasMaxLength(150);
            entity.Property(p => p.Telefono).HasMaxLength(50);
        });

        // ProductoProveedor
        modelBuilder.Entity<ProductoProveedor>(entity =>
        {
            entity.ToTable("ProductoProveedor");
            entity.HasKey(pp => pp.Id);
            entity.Property(pp => pp.Precio).HasColumnType("DECIMAL(10,2)");
            entity.Property(pp => pp.Stock).IsRequired();
            entity.Property(pp => pp.Lote).HasMaxLength(50);
            entity.HasOne<Producto>().WithMany().HasForeignKey(pp => pp.ProductoId);
            entity.HasOne<Proveedor>().WithMany().HasForeignKey(pp => pp.ProveedorId);
        });

        // ListaDeseos
        modelBuilder.Entity<ListaDeseo>(entity =>
        {
            entity.ToTable("ListaDeseos");
            entity.HasKey(ld => ld.Id);
            entity.Property(ld => ld.FechaAgregado).HasDefaultValueSql("GETDATE()");
            entity.HasOne<Usuario>().WithMany().HasForeignKey(ld => ld.UsuarioId);
            entity.HasOne<Producto>().WithMany().HasForeignKey(ld => ld.ProductoId);
        });

        // Compras
        modelBuilder.Entity<Compra>(entity =>
        {
            entity.ToTable("Compras");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.PrecioCompra).HasColumnType("DECIMAL(10,2)");
            entity.Property(c => c.Cantidad).IsRequired();
            entity.Property(c => c.FechaCompra).HasDefaultValueSql("GETDATE()");
            entity.HasOne<Usuario>().WithMany().HasForeignKey(c => c.UsuarioId);
            entity.HasOne<Producto>().WithMany().HasForeignKey(c => c.ProductoId);
        });
    }
}

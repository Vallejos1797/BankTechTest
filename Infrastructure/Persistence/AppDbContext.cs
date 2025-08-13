using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();               // 👈 NECESARIO

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(b =>
        {
            b.ToTable("Products");
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).HasMaxLength(100).IsRequired();
            b.Property(x => x.Description).HasMaxLength(255).IsRequired();
            b.Property(x => x.Logo).HasMaxLength(255).IsRequired();
            b.Property(x => x.DateRelease).IsRequired();
            b.Property(x => x.DateRevision).IsRequired();
        });

        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.HasKey(x => x.Id);
            b.Property(x => x.Username).HasMaxLength(50).IsRequired();
            b.HasIndex(x => x.Username).IsUnique();
            b.Property(x => x.Email).HasMaxLength(120).IsRequired();
            b.Property(x => x.PasswordHash).IsRequired();
            b.Property(x => x.PasswordSalt).IsRequired();
            b.Property(x => x.Role).HasMaxLength(20).HasDefaultValue("user");
            b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        });
    }
}
using FurnitureManager.Models;
using Microsoft.EntityFrameworkCore;

namespace FurnitureManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Client.Name unic (nu vrei mai multi clienti cu acelasi nume)
        modelBuilder.Entity<Client>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<Client>()
            .Property(c => c.Name)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<Project>()
            .Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<Project>()
            .Property(p => p.Revenue)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Client)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

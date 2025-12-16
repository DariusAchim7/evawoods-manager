using FurnitureManager.Models;
using Microsoft.EntityFrameworkCore;

namespace FurnitureManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ExpenseCategory> ExpenseCategories => Set<ExpenseCategory>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

        modelBuilder.Entity<ExpenseCategory>()
            .Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<ExpenseCategory>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .HasMaxLength(120)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.ExpenseCategoryId, p.Name })
            .IsUnique();

        modelBuilder.Entity<Product>()
            .HasOne(p => p.ExpenseCategory)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.ExpenseCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Expense>()
            .Property(e => e.Qty)
            .HasPrecision(18, 4);

        modelBuilder.Entity<Expense>()
            .Property(e => e.UnitPrice)
            .HasPrecision(18, 4);

        modelBuilder.Entity<Expense>()
            .Property(e => e.Note)
            .HasMaxLength(500);

        modelBuilder.Entity<Expense>()
            .HasOne(e => e.Project)
            .WithMany()
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Expense>()
            .HasOne(e => e.ExpenseCategory)
            .WithMany()
            .HasForeignKey(e => e.ExpenseCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Expense>()
            .HasOne(e => e.Product)
            .WithMany()
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed minim ca sa ai dropdown-uri populabile din prima
        modelBuilder.Entity<ExpenseCategory>().HasData(
            new ExpenseCategory { Id = 1, Name = "Lemn" },
            new ExpenseCategory { Id = 2, Name = "Feronerie" },
            new ExpenseCategory { Id = 3, Name = "Suruburi" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, ExpenseCategoryId = 1, Name = "MDF" },
            new Product { Id = 2, ExpenseCategoryId = 1, Name = "PAL" },
            new Product { Id = 3, ExpenseCategoryId = 1, Name = "PFL" },
            new Product { Id = 4, ExpenseCategoryId = 2, Name = "Balamale" },
            new Product { Id = 5, ExpenseCategoryId = 2, Name = "Ghidaje sertar" },
            new Product { Id = 6, ExpenseCategoryId = 3, Name = "Surub 3.5x16" }
        );
    }
}

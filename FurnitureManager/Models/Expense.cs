using System.ComponentModel.DataAnnotations;

namespace FurnitureManager.Models;

public class Expense
{
    public int Id { get; set; }

    [Required]
    public int ProjectId { get; set; }

    public Project? Project { get; set; }

    [Required]
    public int ExpenseCategoryId { get; set; }

    public ExpenseCategory? ExpenseCategory { get; set; }

    [Required]
    public int ProductId { get; set; }

    public Product? Product { get; set; }

    [Range(0.0001, double.MaxValue)]
    public decimal Qty { get; set; }

    [Range(0.0001, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    [MaxLength(500)]
    public string? Note { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Calculat la runtime (nu in DB)
    public decimal Amount => Qty * UnitPrice;
}

using System.ComponentModel.DataAnnotations;

namespace FurnitureManager.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public int ExpenseCategoryId { get; set; }

    public ExpenseCategory? ExpenseCategory { get; set; }

    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;
}

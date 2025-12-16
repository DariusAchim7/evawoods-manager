using System.ComponentModel.DataAnnotations;

namespace FurnitureManager.Models;

public class ExpenseCategory
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public List<Product> Products { get; set; } = new();
}

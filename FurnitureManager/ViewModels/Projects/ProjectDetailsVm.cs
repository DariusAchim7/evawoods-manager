using FurnitureManager.Models;

namespace FurnitureManager.ViewModels.Projects;

public class ProjectDetailsVm
{
    public Client Client { get; set; } = default!;
    public Project Project { get; set; } = default!;

    public List<Expense> Expenses { get; set; } = new();

    public List<ExpenseCategory> Categories { get; set; } = new();
    public List<Product> Products { get; set; } = new();

    // Input pentru cheltuiala noua
    public int NewExpenseCategoryId { get; set; }
    public int NewProductId { get; set; }
    public decimal NewQty { get; set; }
    public decimal NewUnitPrice { get; set; }
    public string? NewNote { get; set; }

    public decimal TotalExpenses
    {
    get
    {
        if (Expenses == null || Expenses.Count == 0)
            return 0m;

        return Expenses.Sum(e => e.Qty * e.UnitPrice);
    }
    }
    public decimal Profit
    {
    get
    {
        if (Project == null)
            return 0m;

        return Project.Revenue - TotalExpenses;
    }
    }
}

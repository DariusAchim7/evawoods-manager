using FurnitureManager.Models;

namespace FurnitureManager.ViewModels.Projects;

public class ProjectDetailsVm
{
    public Client Client { get; set; } = default!;
    public Project Project { get; set; } = default!;

    // Placeholder. Cand introducem cheltuieli, calculam real.
    public decimal TotalExpenses { get; set; }
    public decimal Profit => Project.Revenue - TotalExpenses;
}

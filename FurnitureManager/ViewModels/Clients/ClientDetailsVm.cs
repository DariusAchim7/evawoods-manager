using FurnitureManager.Models;

namespace FurnitureManager.ViewModels.Clients;

public class ClientDetailsVm
{
    public Client Client { get; set; } = default!;
    public List<Project> Projects { get; set; } = new();

    // Pentru formularul de adaugare proiect
    public string NewProjectName { get; set; } = string.Empty;
}

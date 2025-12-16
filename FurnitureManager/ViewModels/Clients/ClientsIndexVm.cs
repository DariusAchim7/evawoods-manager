using FurnitureManager.Models;

namespace FurnitureManager.ViewModels.Clients;

public class ClientsIndexVm
{
    public string? Search { get; set; }
    public List<Client> Clients { get; set; } = new();

    // Pentru formularul de adaugare
    public string NewClientName { get; set; } = string.Empty;

    // Mesaj simplu pentru erori (ex: nume duplicat)
    public string? ErrorMessage { get; set; }
}

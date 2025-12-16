using System.ComponentModel.DataAnnotations;

namespace FurnitureManager.Models;

public class Project
{
    public int Id { get; set; }

    [Required]
    public int ClientId { get; set; }

    public Client? Client { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    // Incasat (total) pentru proiect. Mai tarziu putem face si incasari multiple, daca vrei.
    [Range(0, double.MaxValue)]
    public decimal Revenue { get; set; }
}

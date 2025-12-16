using System.ComponentModel.DataAnnotations;

namespace FurnitureManager.Models;

public class Client
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public List<Project> Projects { get; set; } = new();
}
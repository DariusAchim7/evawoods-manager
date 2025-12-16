using FurnitureManager.Data;
using FurnitureManager.ViewModels.Projects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnitureManager.Controllers;

[Route("Clients/{clientId:int}/Projects")]
public class ProjectsController : Controller
{
    private readonly AppDbContext _db;

    public ProjectsController(AppDbContext db)
    {
        _db = db;
    }

    // Ruta ierarhica:
    // /Clients/{clientId}/Projects/{projectId}
    [HttpGet("{projectId:int}")]
    public async Task<IActionResult> Details(int clientId, int projectId)
    {
        var client = await _db.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == clientId);

        if (client is null) return NotFound();

        var project = await _db.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == projectId && p.ClientId == clientId);

        if (project is null) return NotFound();

        var vm = new ProjectDetailsVm
        {
            Client = client,
            Project = project,
            TotalExpenses = 0m
        };

        return View(vm);
    }
}

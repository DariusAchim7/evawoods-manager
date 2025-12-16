using FurnitureManager.Data;
using FurnitureManager.Models;
using FurnitureManager.ViewModels.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnitureManager.Controllers;

public class ClientsController : Controller
{
    private readonly AppDbContext _db;

    public ClientsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search)
    {
        var query = _db.Clients.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(c => EF.Functions.ILike(c.Name, $"%{s}%"));
        }

        var clients = await query
            .OrderBy(c => c.Name)
            .ToListAsync();

        var vm = new ClientsIndexVm
        {
            Search = search,
            Clients = clients
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClientsIndexVm input)
    {
        var name = (input.NewClientName ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            input.ErrorMessage = "Numele clientului este obligatoriu.";
            return await IndexWithError(input);
        }

        _db.Clients.Add(new Client { Name = name });

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Cel mai probabil index unic pe Client.Name
            input.ErrorMessage = "Exista deja un client cu acest nume.";
            return await IndexWithError(input);
        }

        return RedirectToAction(nameof(Index), new { search = input.Search });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var client = await _db.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (client is null) return NotFound();

        var projects = await _db.Projects
            .AsNoTracking()
            .Where(p => p.ClientId == id)
            .OrderByDescending(p => p.Id)
            .ToListAsync();

        var vm = new ClientDetailsVm
        {
            Client = client,
            Projects = projects
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProject(int id, ClientDetailsVm input)
    {
        var name = (input.NewProjectName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            // Reincarcam pagina clientului (fara mesaj fancy inca)
            return RedirectToAction(nameof(Details), new { id });
        }

        var clientExists = await _db.Clients.AnyAsync(c => c.Id == id);
        if (!clientExists) return NotFound();

        _db.Projects.Add(new Project
        {
            ClientId = id,
            Name = name,
            Revenue = 0m
        });

        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id });
    }

    private async Task<IActionResult> IndexWithError(ClientsIndexVm input)
    {
        // Reincarc lista, pastrand search + mesaj
        var query = _db.Clients.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(input.Search))
        {
            var s = input.Search.Trim();
            query = query.Where(c => EF.Functions.ILike(c.Name, $"%{s}%"));
        }

        input.Clients = await query.OrderBy(c => c.Name).ToListAsync();
        return View("Index", input);
    }
}

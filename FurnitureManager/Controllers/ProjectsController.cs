using FurnitureManager.Data;
using FurnitureManager.Models;
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

    // /Clients/{clientId}/Projects/{projectId}
    [HttpGet("{projectId:int}")]
    public async Task<IActionResult> Details(int clientId, int projectId)
    {
        var client = await _db.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == clientId);
        if (client is null) return NotFound();

        var project = await _db.Projects.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == projectId && p.ClientId == clientId);
        if (project is null) return NotFound();

        var expenses = await _db.Expenses.AsNoTracking()
            .Where(e => e.ProjectId == projectId)
            .Include(e => e.ExpenseCategory)
            .Include(e => e.Product)
            .OrderByDescending(e => e.Id)
            .ToListAsync();

        var categories = await _db.ExpenseCategories.AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();

        var products = await _db.Products.AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync();

        var defaultCategoryId = categories.FirstOrDefault()?.Id ?? 0;
        var defaultProductId = products.FirstOrDefault(p => p.ExpenseCategoryId == defaultCategoryId)?.Id ?? 0;

        var vm = new ProjectDetailsVm
        {
            Client = client,
            Project = project,
            Expenses = expenses,
            Categories = categories,
            Products = products,

            NewExpenseCategoryId = defaultCategoryId,
            NewProductId = defaultProductId
        };

        return View(vm);
    }

    // POST: /Clients/{clientId}/Projects/{projectId}/Expenses
    [HttpPost("{projectId:int}/Expenses")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddExpense(int clientId, int projectId, ProjectDetailsVm input)
    {
        var projectOk = await _db.Projects.AnyAsync(p => p.Id == projectId && p.ClientId == clientId);
        if (!projectOk) return NotFound();

        if (input.NewQty <= 0 || input.NewUnitPrice <= 0)
            return RedirectToAction(nameof(Details), new { clientId, projectId });

        var productOk = await _db.Products.AnyAsync(p =>
            p.Id == input.NewProductId && p.ExpenseCategoryId == input.NewExpenseCategoryId);

        if (!productOk)
            return RedirectToAction(nameof(Details), new { clientId, projectId });

        var expense = new Expense
        {
            ProjectId = projectId,
            ExpenseCategoryId = input.NewExpenseCategoryId,
            ProductId = input.NewProductId,
            Qty = input.NewQty,
            UnitPrice = input.NewUnitPrice,
            Note = string.IsNullOrWhiteSpace(input.NewNote) ? null : input.NewNote.Trim()
        };

        _db.Expenses.Add(expense);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { clientId, projectId });
    }
}

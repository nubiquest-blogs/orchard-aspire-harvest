using BackendApp.Database;
using DemoContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.Controllers;

[ApiController]
public class CategoriesController(AppDbContext db) : ControllerBase
{
    [HttpGet("categories/{id}")]
    public async Task<IActionResult> GetCategory(string id, CancellationToken token = default)
    {
        var category = await db.Categories.FirstOrDefaultAsync(e => e.Id == id, token);

        return category != null ? Ok(new CategoryResponse() { Id = category.Id, Name = category.Name }) : NotFound();
    }
}
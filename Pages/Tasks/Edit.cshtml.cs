using Managingtasks.Data;
using Managingtasks.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Managingtasks.Pages.Tasks;

[Authorize]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;

    [BindProperty]
    public TaskItem Task { get; set; } = new();

    public EditModel(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult OnGet(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var task = _db.Tasks.FirstOrDefault(t =>
            t.Id == id &&
            t.UserId == userId);

        if (task == null)
        {
            return NotFound();
        }

        Task = task;

        return Page();
    }

    public IActionResult OnPost()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var taskInDb = _db.Tasks.FirstOrDefault(t =>
            t.Id == Task.Id &&
            t.UserId == userId);

        if (taskInDb == null)
        {
            return NotFound();
        }

        taskInDb.Title = Task.Title;
        taskInDb.Description = Task.Description;
        taskInDb.Status = Task.Status;

        _db.SaveChanges();

        return RedirectToPage("/Tasks/Index");
    }
}
using Managingtasks.Data;
using Managingtasks.Models.Entities;
using Managingtasks.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Managingtasks.Pages.Tasks;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;

    public List<TaskItem> Tasks { get; set; } = new();

    [BindProperty]
    public TaskItem TaskItem { get; set; } = new();

    public string CurrentSort { get; set; } = string.Empty;
    public string StatusSort { get; set; } = string.Empty;
    public string PrioritySort { get; set; } = string.Empty;
    public string DateSort { get; set; } = string.Empty;

    public IndexModel(ApplicationDbContext db)
    {
        _db = db;
    }

    public void OnGet(string? sortBy)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        CurrentSort = sortBy ?? "date_desc";
        StatusSort = sortBy == "status_asc" ? "status_desc" : "status_asc";
        PrioritySort = sortBy == "priority_asc" ? "priority_desc" : "priority_asc";
        DateSort = sortBy == "date_desc" || string.IsNullOrEmpty(sortBy) ? "date_asc" : "date_desc";

        var query = _db.Tasks.Where(t => t.UserId == userId);

        query = sortBy switch
        {
            "status_asc" => query.OrderBy(t => t.Status),
            "status_desc" => query.OrderByDescending(t => t.Status),
            "priority_asc" => query.OrderBy(t => t.Priority),
            "priority_desc" => query.OrderByDescending(t => t.Priority),
            "date_asc" => query.OrderBy(t => t.CreatedAt),
            "date_desc" => query.OrderByDescending(t => t.CreatedAt),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };

        Tasks = query.ToList();
    }
    // public IActionResult OnPostadd()
    // {
    //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    //     TaskItem.UserId = userId;
    //     TaskItem.CreatedAt = DateTime.UtcNow;
    //     TaskItem.UpdatedAt = DateTime.UtcNow;

    //     _db.Tasks.Add(TaskItem);
    //     _db.SaveChanges();

    //     return RedirectToPage("Index");
    // }in case it will be needed in the future
    public IActionResult OnPostDelete(int id){
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      var task = _db.Tasks.FirstOrDefault(t => t.Id == id);

      if (task == null || task.UserId != userId)
      {
          return NotFound(); // אבטחה
      }

      _db.Tasks.Remove(task);
      _db.SaveChanges();

      return RedirectToPage();
    }

    public IActionResult OnPostEdit(int id, TaskItemStatus status)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var task = _db.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);

        if (task == null)
        {
            return NotFound();
        }

        task.Status = status;
        task.UpdatedAt = DateTime.UtcNow;
        _db.SaveChanges();

        return RedirectToPage();
    }
}

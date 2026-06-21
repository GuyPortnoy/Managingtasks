using System.ComponentModel.DataAnnotations;
using Managingtasks.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Managingtasks.Models.Entities;

public class TaskItem
{
    public int Id { get; set; }
    
    [Required]
    [MinLength(2)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [Required]
    [ValidateNever]
    public string UserId { get; set; } = null!;
    
}


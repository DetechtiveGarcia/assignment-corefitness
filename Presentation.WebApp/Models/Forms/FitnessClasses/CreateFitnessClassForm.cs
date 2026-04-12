using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Forms.FitnessClasses;

public class CreateFitnessClassForm
{
    [Required]
    [Display(Name = "Title")]
    public string Title { get; set; } = null!;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Instructor Name")]
    public string? InstructorName { get; set; }

    [Display(Name = "Category")]
    public string? Category { get; set; }

    [Required]
    [Display(Name = "Start Time")]
    public DateTime StartTime { get; set; }

    [Required]
    [Display(Name = "End Time")]
    public DateTime EndTime { get; set; }
}
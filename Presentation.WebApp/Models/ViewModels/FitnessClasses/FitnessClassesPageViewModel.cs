using Application.Dtos.ClassBookings;
using Application.Dtos.FitnessClasses;
using Presentation.WebApp.Models.Forms.FitnessClasses;

namespace Presentation.WebApp.Models.ViewModels.FitnessClasses;

public class FitnessClassesPageViewModel
{
    public IEnumerable<FitnessClassDetails> Classes { get; set; } = [];
    public IEnumerable<ClassBookingDetails> MyBookings { get; set; } = [];
    public CreateFitnessClassForm Form { get; set; } = new();
}
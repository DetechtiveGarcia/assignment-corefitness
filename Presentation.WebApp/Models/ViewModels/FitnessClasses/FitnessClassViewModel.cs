using Application.Dtos.FitnessClasses;
using Presentation.WebApp.Models.Forms.FitnessClasses;

namespace Presentation.WebApp.Models.ViewModels.FitnessClasses;

public class FitnessClassesViewModel
{
    public IEnumerable<FitnessClassDetails> Classes { get; set; } = [];
    public CreateFitnessClassForm Form { get; set; } = new();
}
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Forms.Authentication;

public class SignUpEmailForm
{
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email *", Prompt = "username@example.com")]
    public string Email { get; set; } = null!;


    public string? ErrorMessage { get; set; }
}

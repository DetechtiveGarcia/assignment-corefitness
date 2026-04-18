using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Forms.Authentication;

public class SignInForm
{
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email *", Prompt = "username@example.com")]
    public string Email { get; set; } = null!;
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password *", Prompt = "Enter your password")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }

    public string? ErrorMessage { get; set; }
}

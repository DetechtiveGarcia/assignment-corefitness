using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Forms.Authentication;

public class SignUpPasswordForm
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password *", Prompt = "Enter your password")]
    public string Password { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password *", Prompt = "Confirm your password")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = null!;


    //[Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the user terms and conditions.")]
    public bool AcceptTermsAndConditions { get; set; }
    public string? ErrorMessage { get; set; }
}

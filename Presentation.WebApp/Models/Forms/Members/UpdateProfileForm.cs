using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Forms.Members;

public class UpdateProfileForm
{
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
    [Display(Name = "First Name", Prompt = "Enter First Name")]
    public string? FirstName { get; set; }

    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
    [Display(Name = "Last Name", Prompt = "Enter Last Name")]
    public string? LastName { get; set; }

    [Phone(ErrorMessage = "Enter a valid phone number.")]
    [Display(Name = "Phone Number", Prompt = "Enter Phone Number")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Profile Image")]
    public IFormFile? ProfileImage { get; set; }

    public string? ExistingProfileImageUrl { get; set; }
}

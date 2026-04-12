using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Forms.Memberships;

public class CreateMembershipForm
{
    [Required(ErrorMessage = "You must select a membership.")]
    [Display(Name = "Membership")]
    public string MembershipName { get; set; } = null!;
}
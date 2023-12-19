using System.ComponentModel.DataAnnotations;
using DotNet8Auth.Shared.Models.Authentication.Login;

namespace DotNet8Auth.Shared.Models.Authentication.SetPhoneNumber;

public class SetPhoneNumberInputModel : BaseInputModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = "";
        
    [Phone]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; } = "";
}
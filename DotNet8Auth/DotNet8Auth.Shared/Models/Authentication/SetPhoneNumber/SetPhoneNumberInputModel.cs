using System.ComponentModel.DataAnnotations;

namespace DotNet8Auth.Shared.Models.Authentication.SetPhoneNumber;

public class SetPhoneNumberInputModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = "";
        
    [Phone]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; } = "";
}
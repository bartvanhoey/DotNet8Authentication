using System.ComponentModel.DataAnnotations;
using DotNet8Auth.Shared.Models.Authentication.Login;

namespace DotNet8Auth.Shared.Models.Authentication.SetPhoneNumber;

public class SetPhoneNumberInputModel : BaseInputModel
{
    [Phone]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; } = "";
}
using System.ComponentModel.DataAnnotations;
using DotNet8Auth.Shared.Models.Authentication.Login;

namespace DotNet8Auth.Shared.Models.Authentication.ResendEmailConfirmation;

public class ResendEmailConfirmationInputModel : BaseInputModel
{
    [Required] [EmailAddress] public string Email { get; set; } = "";
   
}
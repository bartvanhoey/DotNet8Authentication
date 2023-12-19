using System.ComponentModel.DataAnnotations;
using DotNet8Auth.Shared.Models.Authentication.Login;

namespace DotNet8Auth.Shared.Models.Authentication.Profile;

public class GetProfileInputModel : BaseInputModel
{
    [Required] [EmailAddress] public string Email { get; set; } = "";
}
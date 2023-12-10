using System.ComponentModel.DataAnnotations;

namespace DotNet8Auth.Shared.Models.Authentication.Profile
{
    public class GetProfileInputModel
    {
        [Required] [EmailAddress] public string Email { get; set; } = "";
    }
}
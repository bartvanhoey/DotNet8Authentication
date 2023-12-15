using System.ComponentModel.DataAnnotations;

namespace DotNet8Auth.Shared.Models.Authentication.ForgotPassword
{
    public class ForgotPasswordInputModel
    {
        [Required] [EmailAddress] public string Email { get; set; } = "";
    }
}
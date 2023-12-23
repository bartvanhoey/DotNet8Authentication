using System.ComponentModel.DataAnnotations;
using DotNet8Auth.Shared.Models.Authentication.Login;

namespace DotNet8Auth.Shared.Models.Authentication.ChangeEmail
{
    public class ConfirmChangeEmailInputModel : BaseInputModel
    {
        [Required][EmailAddress] public string NewEmail { get; set; } = "";
        [Required] public string Code { get; set; } = "";
    }
}
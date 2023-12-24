using System.ComponentModel.DataAnnotations;
using DotNet8Auth.Shared.Models.Authentication.Login;
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

public class ConfirmChangeEmailInputModel : BaseInputModel
{
    public ConfirmChangeEmailInputModel()
    {
    }

    public ConfirmChangeEmailInputModel(string email, string newEmail, string code)
    {
        Email = email;
        NewEmail = newEmail;
        Code = code;
    }

    [Required][EmailAddress] public string Email { get; set; }
    [Required][EmailAddress] public string NewEmail { get; set; } = "";
    [Required] public string Code { get; set; } = "";
}
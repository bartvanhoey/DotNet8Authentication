using System.ComponentModel.DataAnnotations;
using DotNet8Auth.Shared.Models.Authentication.Login;
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

public class ConfirmChangeEmailInputModel : BaseInputModel
{
    public ConfirmChangeEmailInputModel()
    {
    }

    public ConfirmChangeEmailInputModel(string newEmail, string code)
    {
        NewEmail = newEmail;
        Code = code;
    }

    [Required][EmailAddress] public string NewEmail { get; set; } = "";
    [Required] public string Code { get; set; } = "";
}
using System.ComponentModel.DataAnnotations;
using DotNet8Auth.Shared.Models.Authentication.Login;

namespace DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

public class ChangeEmailInputModel : BaseInputModel
{
    public ChangeEmailInputModel()
    {
    }

    public ChangeEmailInputModel(string? newEmail)
    {
        NewEmail = newEmail;
    }

    [Required][EmailAddress] public string? NewEmail { get; set; } = "";
}
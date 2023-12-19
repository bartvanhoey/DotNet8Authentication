using DotNet8Auth.Shared.Models.Authentication.Login;

namespace DotNet8Auth.Shared.Models.Authentication.ConfirmEmail;

public class ConfirmEmailInputModel : BaseInputModel
{
    public string UserId { get; set; } = "";
    public string Code { get; set; } = "";
}
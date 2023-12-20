using DotNet8Auth.Shared.Models.Authentication.Login;

namespace DotNet8Auth.Shared.Models.Authentication.ConfirmEmail;

public class ConfirmEmailInputModel(string userId, string code) : BaseInputModel
{
    public string UserId { get; } = userId;
    public string Code { get; } = code;
}
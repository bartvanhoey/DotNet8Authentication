namespace DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

public class ConfirmChangeEmailError(string code, string description)
{
    public string Code { get; } = code;
    public string Description { get; } = description;
}
namespace DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

public class ControllerResponseError(string code, string description)
{
    public string Code { get; } = code;
    public string Description { get; } = description;
}
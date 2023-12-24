namespace DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

public class ChangeEmailResponse(string status, string message)
{
    public string? Status { get; set; } = status;

    public string? Message { get; set; } = message;
}
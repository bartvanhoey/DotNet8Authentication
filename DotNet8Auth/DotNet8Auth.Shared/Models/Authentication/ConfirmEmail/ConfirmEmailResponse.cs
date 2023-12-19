namespace DotNet8Auth.Shared.Models.Authentication.ConfirmEmail;

public class ConfirmEmailResponse
{
    public ConfirmEmailResponse()
    {
    }

    public ConfirmEmailResponse(string? status, string? message)
    {
        Status = status;
        Message = message;
    }

    public string? Status { get; set; }
    public string? Message { get; set; }
    public string? Code { get; set; }
    public string? UserId { get; set; }
}
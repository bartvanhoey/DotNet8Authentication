namespace DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

public class IsEmailConfirmedResponse(string? status, bool isEmailConfirmed)
{
    public IsEmailConfirmedResponse(string? status, string? message) 
        : this(status, false) => Message = message;

    public string? Status { get; set; } = status;
    public string? Message { get; set; }
    public bool IsEmailConfirmed { get; set; } = isEmailConfirmed;
}
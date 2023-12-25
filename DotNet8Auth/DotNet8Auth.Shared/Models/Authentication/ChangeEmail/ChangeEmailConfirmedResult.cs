namespace DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

public class ChangeEmailConfirmedResult : IResponseContentResult
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public IEnumerable<HttpResultError>? Errors { get; set; }
    public bool Succeeded => Status == "Success";
    public bool IsEmailConfirmed { get; set; }
}
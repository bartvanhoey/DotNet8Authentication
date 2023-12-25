namespace DotNet8Auth.Shared.Models.Authentication.ResetPassword;

public class ResetPasswordResult : IResponseContentResult
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public IEnumerable<HttpResultError>? Errors { get; set; }
    public bool Succeeded => Status == "Success";
}
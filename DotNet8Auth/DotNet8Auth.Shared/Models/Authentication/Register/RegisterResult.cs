namespace DotNet8Auth.Shared.Models.Authentication.Register;

public class RegisterResult : IResponseContentResult
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public IEnumerable<HttpResultError>? Errors { get; set; }
   
    public bool Succeeded => Status == "Success";
}
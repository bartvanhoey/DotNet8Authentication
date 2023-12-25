namespace DotNet8Auth.Shared.Models.Authentication.Profile;

public class ProfileResult : IResponseContentResult
{
    public string? UserName { get; set; } 
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? PhoneNumber { get; set; } 
    public string? Message { get; set; }
    public IEnumerable<HttpResultError>? Errors { get; set; }
    public bool Succeeded => Status == "Success";
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? Status { get; set; }

}
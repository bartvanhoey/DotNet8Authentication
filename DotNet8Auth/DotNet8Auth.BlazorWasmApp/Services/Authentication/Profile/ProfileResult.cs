namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Profile;

public class ProfileResult
{
    public string? UserName { get; set; } 
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? PhoneNumber { get; set; } 
    public string? Message { get; set; }
    public bool Succeeded => Status == "Success";
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? Status { get; set; }

}
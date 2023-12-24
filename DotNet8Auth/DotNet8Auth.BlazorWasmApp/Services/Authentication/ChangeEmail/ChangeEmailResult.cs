namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangeEmail;

public class ChangeEmailResult
{
    public string? Message { get; set; }
    public bool Succeeded => Status == "Success";
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? Status { get; set; }

}
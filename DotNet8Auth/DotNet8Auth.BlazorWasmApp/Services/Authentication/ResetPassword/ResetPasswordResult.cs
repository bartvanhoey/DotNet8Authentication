namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ResetPassword;

public class ResetPasswordResult
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public bool Succeeded => Status == "Success";
}
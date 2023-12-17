namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Register;

public class RegisterResult
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public bool Succeeded => Status == "Success";
}
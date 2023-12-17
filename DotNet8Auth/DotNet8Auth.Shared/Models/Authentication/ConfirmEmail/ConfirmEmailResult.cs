namespace DotNet8Auth.Shared.Models.Authentication.ConfirmEmail;

public class ConfirmEmailResult
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public bool Succeeded => Status == "Success";
}
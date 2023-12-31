namespace DotNet8Auth.Shared.Models.Authentication.ChangePassword;

public class UserHasPasswordResult
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public bool Succeeded => Status == "Success";
    public bool UserHasPassword { get; set; }
}
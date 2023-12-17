namespace DotNet8Auth.Shared.Models.Authentication.ResetPassword;

public class ResetPasswordResponse
{
    public string? Status { get; set; }
    public string? Code { get; set; }
    public string? UserId { get; set; }
    public string? Message { get; set; }

}
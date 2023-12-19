namespace DotNet8Auth.Shared.Models.Authentication.ResetPassword;

public class ResetPasswordResponse
{
    public ResetPasswordResponse()
    {
    }

    public ResetPasswordResponse(string? status, string? message)
    {
        Status = status;
        Message = message;
    }

    public string? Status { get; set; }
    public string? Code { get; set; }
    public string? UserId { get; set; }
    public string? Message { get; set; }

}
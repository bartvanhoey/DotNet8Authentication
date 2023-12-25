using DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

namespace DotNet8Auth.Shared.Models.Authentication.Register;

public class RegisterResponse : IControllerResponse
{
    public RegisterResponse()
    {
    }

    public RegisterResponse(string? status, string? message)
    {
        Status = status;
        Message = message;
    }

    public RegisterResponse(string? status, string? code, string? userId) : this()
    {
        Status = status;
        Message = status == "Success"? "User created successfully" : "User Not created";
        Code = code;
        UserId = userId;
        
    }

    public string? Status { get; set; }
    public string? Code { get; set; }
    public string? UserId { get; set; }
    public string? Message { get; set; }
    public IEnumerable<ControllerResponseError>? Errors { get; set; }
}
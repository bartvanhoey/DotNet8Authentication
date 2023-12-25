namespace DotNet8Auth.Shared.Models.Authentication.Profile;

public class ProfileResponse : IControllerResponse
{
    public ProfileResponse()
    {
        
    }

    public ProfileResponse(string? status, string? message)
    {
        Status = status;
        Message = message;
    }

    public ProfileResponse(string? status, string? userName, string? phoneNumber)
    {
        Status = status;
        UserName = userName;
        PhoneNumber = phoneNumber;
    }
    
    public string? Status { get; set; } 
    public string? Message { get; set; }
    public IEnumerable<ControllerResponseError>? Errors { get; set; }
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }
}
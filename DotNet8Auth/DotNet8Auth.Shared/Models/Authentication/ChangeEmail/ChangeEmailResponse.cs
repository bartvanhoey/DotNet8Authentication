namespace DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

public class ChangeEmailResponse :IControllerResponse
{
    public ChangeEmailResponse()
    {
        
    }

    public ChangeEmailResponse(string status, string message)
    {
        Status = status;
        Message = message;
    }
    
    
    public string? Status { get; set; } 

    public string? Message { get; set; } 
    public IEnumerable<ControllerResponseError>? Errors { get; set; }
}
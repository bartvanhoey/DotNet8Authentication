namespace DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

public class IsEmailConfirmedResponse : IControllerResponse
{
    public IsEmailConfirmedResponse()
    {
        
    }
    
    public IsEmailConfirmedResponse(string? status, string? message)
    {
        Status = status;
        Message = message;
    }
    
    public IsEmailConfirmedResponse(string? status,bool isEmailConfirmed)
    {
        Status = status;
        IsEmailConfirmed = isEmailConfirmed;
    }


    public string? Status { get; set; } 
    public string? Message { get; set; }
    public IEnumerable<ControllerResponseError>? Errors { get; set; }
    public bool IsEmailConfirmed { get; set; }
}
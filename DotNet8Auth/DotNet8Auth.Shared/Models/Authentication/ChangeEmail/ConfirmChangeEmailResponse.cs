namespace DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

public interface IControllerResponse
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public IEnumerable<ControllerResponseError>? Errors { get; set; }
}

public class ConfirmChangeEmailResponse : IControllerResponse
{
    public ConfirmChangeEmailResponse()
    {
        
    }

    public ConfirmChangeEmailResponse(string? status, IEnumerable<ControllerResponseError> errors)
    {
        Status = status;
        Errors = errors;
    }

    public ConfirmChangeEmailResponse(string? status, string? message)
    {
        Status = status;
        Message = message;
    } 
    public string? Status { get; set; } 
    public string? Message { get; set; }
    public IEnumerable<ControllerResponseError>? Errors { get; set; }
}
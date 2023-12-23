namespace DotNet8Auth.Shared.Models.Authentication.ChangeEmail
{
    public class ChangeEmailResponse
    {

        public ChangeEmailResponse() { }

        public ChangeEmailResponse(string status, string message)
        {
            Status = status;
            Message = message;
        }

        public string? Status { get; set; }
        
        public string? Message { get; set; }
    }
}
namespace DotNet8Auth.Shared.Models.Authentication.ConfirmEmail
{
    public class ConfirmEmailResponse
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public string? Code { get; set; }
        public string? UserId { get; set; }
    }
}
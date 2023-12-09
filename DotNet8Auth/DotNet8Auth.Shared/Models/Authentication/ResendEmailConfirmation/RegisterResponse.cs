namespace DotNet8Auth.Shared.Models.Authentication.ResendEmailConfirmation
{
    public class ResendEmailConfirmationResponse
    {
        public string? Status { get; set; }
        public string? Code { get; set; }
        public string? UserId { get; set; }
        public string? Message { get; set; }

    }
}
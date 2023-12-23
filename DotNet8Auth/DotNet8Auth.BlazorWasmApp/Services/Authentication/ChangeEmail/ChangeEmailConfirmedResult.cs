namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangeEmail
{
    public class ChangeEmailConfirmedResult
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public bool Succeeded => Status == "Success";
        public bool IsEmailConfirmed { get; set; }
    }
}
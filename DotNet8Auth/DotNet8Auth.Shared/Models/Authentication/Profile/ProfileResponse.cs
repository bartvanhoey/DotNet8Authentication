namespace DotNet8Auth.Shared.Models.Authentication.Profile
{
    public class ProfileResponse(string? status, string? userName, string? phoneNumber)
    {
        public ProfileResponse(string? status, string? message) : this(status, null, null) => Message = message;

        public string? Status { get; set; } = status;
        public string? Message { get; set; }
        public string? UserName { get; set; } = userName;
        public string? PhoneNumber { get; set; } = phoneNumber;
    }
}
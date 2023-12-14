namespace DotNet8Auth.Shared.Models.Authentication.Login
{
    public class LoginResponse
    {
        public LoginResponse()
        {
        }

        public LoginResponse(string? status, string? message)
        {
            Status = status;
            Message = message;
        }

        public LoginResponse(string? accessToken, DateTime validTo) : this()
        {
            AccessToken = accessToken;
            ValidTo = validTo;
            Successful = true;
        }

        public string? AccessToken { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Successful { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }
        public string? Status { get; set; }
        public string? TraceId { get; set; }
        public string? Message { get; set; }

    }
}
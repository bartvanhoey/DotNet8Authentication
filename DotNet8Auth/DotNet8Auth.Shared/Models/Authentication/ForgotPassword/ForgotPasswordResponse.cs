namespace DotNet8Auth.Shared.Models.Authentication.ForgotPassword
{
    public class ForgotPasswordResponse
    {
        public ForgotPasswordResponse()
        {
        }

        public ForgotPasswordResponse(string? status, string? message)
        {
            Status = status;
            Message = message;
        }

        public ForgotPasswordResponse(string? status, string? message, string? code) : this()
        {
            Status = status;
            Message = message;
            Code = code;
        }

        public string? Status { get; set; }
        public string? Code { get; set; }
        public string? Message { get; set; }

    }
}
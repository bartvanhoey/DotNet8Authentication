// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace DotNet8Auth.Shared.Models.Authentication.ResendEmailConfirmation
{
    public class ResendEmailConfirmationResponse
    {
        public ResendEmailConfirmationResponse()
        {
        }

        public ResendEmailConfirmationResponse(string? status, string? message)
        {
            Status = status;
            Message = message;
        }

        public ResendEmailConfirmationResponse(string? status, string? message, string? code, string? userId) : this()
        {
            Status = status;
            Message = message;
            Code = code;
            UserId = userId;
        }

        public string? Status { get; set; }
        public string? Code { get; set; }
        public string? UserId { get; set; }
        public string? Message { get; set; }

    }
}
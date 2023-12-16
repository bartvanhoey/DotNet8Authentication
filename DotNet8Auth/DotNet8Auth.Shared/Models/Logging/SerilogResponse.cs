namespace DotNet8Auth.Shared.Models.Logging
{
    public class SerilogResponse
    {
        public SerilogResponse()
        {
        }

        public SerilogResponse(string? status, string? message)
        {
            Status = status;
            Message = message;
        }

        public SerilogResponse(string? status, string? message, string? code, string? userId) : this()
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
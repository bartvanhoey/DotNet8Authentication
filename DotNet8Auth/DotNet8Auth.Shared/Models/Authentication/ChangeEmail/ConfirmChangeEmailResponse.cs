namespace DotNet8Auth.Shared.Models.Authentication.ChangeEmail
{
    public class ConfirmChangeEmailResponse(string? status)
    {
        public ConfirmChangeEmailResponse(string? status, IEnumerable<ConfirmChangeEmailError> errors) : this(status) => Errors = errors;

        public ConfirmChangeEmailResponse(string? status, string? message) : this(status) => Message = message;


        public string? Status { get; set; } = status;
        public string? Message { get; set; }

        public IEnumerable<ConfirmChangeEmailError>? Errors { get; set; }

    }
}
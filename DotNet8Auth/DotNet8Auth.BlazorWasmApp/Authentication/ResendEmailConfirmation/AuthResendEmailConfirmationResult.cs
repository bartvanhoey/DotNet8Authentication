using static DotNet8Auth.BlazorWasmApp.Authentication.ResendEmailConfirmation.AuthResendConfirmEmailConfirmationInfo;

namespace DotNet8Auth.BlazorWasmApp.Authentication.ResendEmailConfirmation
{
    public class AuthResendEmailConfirmationResult(AuthResendConfirmEmailConfirmationInfo message)
    {
        public AuthResendEmailConfirmationResult() : this(ResendEmailConfirmationSuccessful)
        {
        }

        public bool Succeeded => Message == ResendEmailConfirmationSuccessful;
        private AuthResendConfirmEmailConfirmationInfo Message { get; set; } = message;
    }
}
using static DotNet8Auth.BlazorWasmApp.Authentication.ResetPassword.AuthResetPasswordInfo;

namespace DotNet8Auth.BlazorWasmApp.Authentication.ResetPassword
{
    public class AuthResetPasswordResult(AuthResetPasswordInfo message)
    {
        public AuthResetPasswordResult() : this(ResetPasswordSuccessful)
        {
        }

        public bool Succeeded => Message == ResetPasswordSuccessful;
        private AuthResetPasswordInfo Message { get; set; } = message;
    }
}
using static DotNet8Auth.BlazorWasmApp.Services.Authentication.ResetPassword.AuthResetPasswordInfo;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ResetPassword
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
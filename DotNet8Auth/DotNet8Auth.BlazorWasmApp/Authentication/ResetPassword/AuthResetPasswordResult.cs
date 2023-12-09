namespace DotNet8Auth.BlazorWasmApp.Authentication.ResetPassword
{
    public class AuthResetPasswordResult(AuthResetPasswordInfo message)
    {
        public AuthResetPasswordResult() : this(AuthResetPasswordInfo.ResetPasswordSuccessful)
        {
        }

        public bool Succeeded => Message == AuthResetPasswordInfo.ResetPasswordSuccessful;
        private AuthResetPasswordInfo Message { get; set; } = message;
    }
}
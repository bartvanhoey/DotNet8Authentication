using static DotNet8Auth.BlazorWasmApp.Services.Authentication.ConfirmEmail.AuthConfirmEmailInfo;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ConfirmEmail
{
    public class AuthConfirmEmailResult(AuthConfirmEmailInfo message)
    {

        public AuthConfirmEmailResult() : this(ConfirmEmailSuccessful)
        {
        }

        public bool Succeeded => Message == ConfirmEmailSuccessful;
        private AuthConfirmEmailInfo Message { get; } = message;
    }


}
using static DotNet8Auth.BlazorWasmApp.Authentication.Register.AuthRegisterInfo;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Register
{
    public class AuthRegisterResult(AuthRegisterInfo message)
    {
        public AuthRegisterResult() : this(RegistrationSuccessful)
        {
        }

        public bool Succeeded => Message == RegistrationSuccessful;
        private AuthRegisterInfo Message { get; set; } = message;
    }

}
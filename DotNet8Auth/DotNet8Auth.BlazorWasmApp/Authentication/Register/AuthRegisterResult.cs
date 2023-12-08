namespace DotNet8Auth.BlazorWasmApp.Authentication.Register
{
    public class AuthRegisterResult
    {
        public AuthRegisterResult() => Message = AuthRegisterInfo.RegistrationSuccessful;
        public AuthRegisterResult(AuthRegisterInfo message) => Message = message;
        public bool Succeeded => Message == AuthRegisterInfo.RegistrationSuccessful;
        public AuthRegisterInfo Message { get; set; }
    }

}
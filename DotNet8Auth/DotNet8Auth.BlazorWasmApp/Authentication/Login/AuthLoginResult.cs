namespace DotNet8Auth.BlazorWasmApp.Authentication.Login
{
    public class AuthLoginResult
    {
        public AuthLoginResult() => Message = AuthLoginMessage.LoginSuccess;
        public AuthLoginResult(AuthLoginMessage message) => Message = message;

        public bool Succeeded => Message == AuthLoginMessage.LoginSuccess;
        public AuthLoginMessage Message { get; set; }
    }

}

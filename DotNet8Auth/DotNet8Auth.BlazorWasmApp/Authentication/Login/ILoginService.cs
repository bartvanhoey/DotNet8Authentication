using DotNet8Auth.Shared.Models.Authentication;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Login
{
    public interface ILoginService
    {
        Task<AuthLoginResult> Login(InputModel inputModel);
    }

    public class AuthLoginResult
    {
        public AuthLoginResult() => Message = AuthLoginMessage.LoginSuccess;
        public AuthLoginResult(AuthLoginMessage message) => Message = message;

        public bool Succeeded => Message == AuthLoginMessage.LoginSuccess;
        public AuthLoginMessage Message { get; set; }
    }

    public enum AuthLoginMessage { 
        LoginSuccess = 0, 
        UnAuthorized = 1,
        Unknown = 2
    }

}

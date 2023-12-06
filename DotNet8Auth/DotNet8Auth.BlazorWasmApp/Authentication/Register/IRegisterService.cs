using DotNet8Auth.Shared.Models.Authentication;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Register
{
    public interface IRegisterService
    {
        Task<AuthRegisterResult> Register(RegisterInputModel inputModel);
    }

    public class AuthRegisterResult
    {
        public AuthRegisterResult() => Message = AuthRegisterInfo.RegistrationSuccessful;
        public AuthRegisterResult(AuthRegisterInfo message) => Message = message;
        public bool Succeeded => Message == AuthRegisterInfo.RegistrationSuccessful;
        public AuthRegisterInfo Message { get; set; }
    }

    public enum AuthRegisterInfo
    {
        RegistrationSuccessful = 0,
        RegistrationUnsuccessful = 1,
    }

}
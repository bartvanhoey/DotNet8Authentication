namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangeEmail
{
    public class AuthIsEmailConfirmedResult
    {

        public AuthIsEmailConfirmedResult(bool isEmailConfirmed){
            Message = AuthIsEmailConfirmedMessage.Success;
            IsEmailConfirmed = isEmailConfirmed;
        }

        public AuthIsEmailConfirmedResult(AuthIsEmailConfirmedMessage message) 
            => Message = message;

        public bool Succeeded => Message == AuthIsEmailConfirmedMessage.Success;
        public AuthIsEmailConfirmedMessage Message { get; set; }

        public bool IsEmailConfirmed { get; set; }
    }
}
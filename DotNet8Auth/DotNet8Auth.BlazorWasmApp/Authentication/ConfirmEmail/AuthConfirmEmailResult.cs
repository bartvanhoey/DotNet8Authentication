namespace DotNet8Auth.BlazorWasmApp.Authentication.ConfirmEmail
{
    public class AuthConfirmEmailResult
    {

        public AuthConfirmEmailResult() => Message = AuthConfirmEmailInfo.ConfirmEmailSuccessful;
        public AuthConfirmEmailResult(AuthConfirmEmailInfo message) => Message = message;
        public bool Succeeded => Message == AuthConfirmEmailInfo.ConfirmEmailSuccessful;
        public AuthConfirmEmailInfo Message { get; set; }
    }


}
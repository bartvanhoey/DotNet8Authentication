using static DotNet8Auth.BlazorWasmApp.Services.Authentication.Login.AuthLoginMessage;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Login;

public class AuthLoginResult
{
    public AuthLoginResult() => Message = LoginSuccess;
    public AuthLoginResult(AuthLoginMessage message) => Message = message;

    public bool Succeeded => Message == LoginSuccess;
    public AuthLoginMessage Message { get; set; }
}
using static DotNet8Auth.BlazorWasmApp.Services.Authentication.Login.AuthLoginMessage;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Login;

public class AuthLoginResult(AuthLoginMessage message)
{
    public AuthLoginResult() : this(LoginSuccess)
    {
    }

    public bool Succeeded => Message == LoginSuccess;
    public AuthLoginMessage Message { get; set; } = message;
}
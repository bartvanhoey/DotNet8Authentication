using static Dotnet8Auth.BlazorServerApp.Services.Authentication.Login.AuthLoginMessage;

namespace Dotnet8Auth.BlazorServerApp.Services.Authentication.Login;

public class AuthLoginResult(AuthLoginMessage message)
{
    public AuthLoginResult() : this(LoginSuccess)
    {
    }
    public bool Succeeded => Message == LoginSuccess;
    public AuthLoginMessage Message { get; set; } = message;
    
    public static AuthLoginResult LoginFailed() => new(SomethingWentWrong);
}
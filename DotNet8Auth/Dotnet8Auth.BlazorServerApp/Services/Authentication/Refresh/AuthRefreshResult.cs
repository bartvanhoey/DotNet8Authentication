using static Dotnet8Auth.BlazorServerApp.Services.Authentication.Refresh.AuthRefreshMessage;

namespace Dotnet8Auth.BlazorServerApp.Services.Authentication.Refresh;

public class AuthRefreshResult
{
    public AuthRefreshResult() => Message = Successful;
    public AuthRefreshResult(AuthRefreshMessage message) => Message = message;
    public AuthRefreshMessage Message { get; set; }
    public bool Succeeded => Message == Successful;
}
using DotNet8Auth.Shared.Models.Authentication.Login;

namespace Dotnet8Auth.BlazorServerApp.Services.Authentication.Login;

public interface ILoginService
{
    Task<AuthLoginResult> Login(LoginInputModel input);
    // Task LogoutAsync();
}
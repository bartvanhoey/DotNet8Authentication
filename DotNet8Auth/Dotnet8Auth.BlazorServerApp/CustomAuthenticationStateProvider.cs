using System.Security.Claims;
using Dotnet8Auth.BlazorServerApp.Services.Authentication;
using Dotnet8Auth.BlazorServerApp.Services.Authentication.Login;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Dotnet8Auth.BlazorServerApp;

public class CustomAuthenticationStateProvider(ProtectedLocalStorage localStorage) : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _localStorage = localStorage;
    private readonly LoginService _loginService;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // var claims = await _loginService.GetLoginInfoAsync();
        // var claimsIdentity = claims.Count != 0 
        //     ? new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme, "name", "role") 
        //     : new ClaimsIdentity();
        // var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        // return new AuthenticationState(claimsPrincipal);

        var identity = new ClaimsIdentity();
        var claims = new List<Claim>();
        var userPrincipal = new ClaimsPrincipal(identity);
        return new AuthenticationState(userPrincipal);
    }


    // Existing constructor and other methods...

    public void SetAuthenticationState(ClaimsPrincipal principal)
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }
    
    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }
    
    public void MarkUserAsAuthenticated(string email)
    {
        var authenticatedUser =
            new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }
}
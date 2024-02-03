using System.Security.Claims;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Refresh;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Token;
using Microsoft.AspNetCore.Components.Authorization;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication;

public class CustomAuthenticationStateProvider(IHttpClientFactory clientFactory, IJwtTokenService jwtTokenService)
    : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        AuthenticationState authenticationState = new(new ClaimsPrincipal(new ClaimsIdentity()));
        try
        {
            var accessToken = await jwtTokenService.GetAccessTokenAsync();
            
            var isTokenValid = jwtTokenService.IsAccessTokenValid(accessToken);
            Console.WriteLine($"accessToken isValid: {isTokenValid}");
            
            var refreshService = new RefreshService(clientFactory, jwtTokenService);
            var refreshResult = await refreshService.RefreshAsync();
            if (refreshResult.Succeeded)
            {
                accessToken = await jwtTokenService.GetAccessTokenAsync();
                isTokenValid = jwtTokenService.IsAccessTokenValid(accessToken);
                Console.WriteLine($"accessToken isValid: {isTokenValid}");
            }

            if (string.IsNullOrWhiteSpace(accessToken) || !isTokenValid)
            {
                await jwtTokenService.RemoveAccessTokenAsync();
                await jwtTokenService.RemoveRefreshTokenAsync();

                NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
                return authenticationState;
            }

            authenticationState =
                new AuthenticationState(
                    new ClaimsPrincipal(new ClaimsIdentity(jwtTokenService.GetClaims(accessToken), "jwt")));
            NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
            return authenticationState;
        }
        catch (Exception)
        {
            NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
            return authenticationState;
        }
    }

    public void MarkUserAsAuthenticated(string email)
    {
        var authenticatedUser =
            new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }
}
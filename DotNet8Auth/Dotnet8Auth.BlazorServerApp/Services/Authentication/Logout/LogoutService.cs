using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Dotnet8Auth.BlazorServerApp.Services.Authentication.Logout;

public class LogoutService(IHttpClientFactory clientFactory, ProtectedLocalStorage localStorage, AuthenticationStateProvider authenticationStateProvider)
    : ILogoutService
{
    public async Task LogoutAsync()
    {
        var httpClient = clientFactory.CreateClient("ServerAPI");
        try
        {
            await httpClient.DeleteAsync("api/account/revoke");
        }
        catch(Exception)
        {
            // TODO logging here
        }
        await localStorage.DeleteAsync("accessToken");
        await localStorage.DeleteAsync("refreshToken");
        await authenticationStateProvider.GetAuthenticationStateAsync();
    }
}
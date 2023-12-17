using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Logout;

public class LogoutService(IHttpClientFactory clientFactory, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
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
        await localStorage.RemoveItemAsync("accessToken");
        await localStorage.RemoveItemAsync("refreshToken");
        await authenticationStateProvider.GetAuthenticationStateAsync();
    }
}
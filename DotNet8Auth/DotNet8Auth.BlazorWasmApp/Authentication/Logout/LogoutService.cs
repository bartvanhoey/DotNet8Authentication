using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Logout
{
    public class LogoutService(IHttpClientFactory clientFactory, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
        : ILogoutService
    {
        public async Task LogoutAsync()
        {
            var httpClient = clientFactory.CreateClient("ServerAPI");
            await httpClient.DeleteAsync("api/account/revoke");
            await localStorage.RemoveItemAsync("accessToken");
            await localStorage.RemoveItemAsync("refreshToken");
            await authenticationStateProvider.GetAuthenticationStateAsync();
        }
    }
}
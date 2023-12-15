using Blazored.LocalStorage;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Logout
{
    public interface ILogoutService
    {
        Task LogoutAsync();
    }

    public class LogoutService(IHttpClientFactory clientFactory, ILocalStorageService localStorage, CustomAuthenticationStateProvider authenticationStateProvider)
        : ILogoutService
    {
        

        public async Task LogoutAsync()
        {
            
            var httpClient = clientFactory.CreateClient("ServerAPI");
            var revokeResponse = await httpClient.DeleteAsync("api/account/revoke");
            await localStorage.RemoveItemAsync("accessToken");
            await localStorage.RemoveItemAsync("refreshToken");
            await authenticationStateProvider.GetAuthenticationStateAsync();
        }
    }
}
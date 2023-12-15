using System.Net.Http.Json;
using Blazored.LocalStorage;
using DotNet8Auth.Shared.Models.Authentication.Refresh;
using static System.String;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Refresh
{
    public class RefreshService(IHttpClientFactory clientFactory, ILocalStorageService localStorage)
    {
        public async Task<AuthRefreshResult> RefreshAsync()
        {
            var accessToken = await localStorage.GetItemAsync<string>("accessToken");
            var refreshToken = await localStorage.GetItemAsync<string>("refreshToken");

            var model = new RefreshInputModel(accessToken, refreshToken);

            var httpClient = clientFactory.CreateClient("ServerAPI");
            var response = await httpClient.PostAsJsonAsync("api/account/refresh", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RefreshResult>();
                if (result == null) return new AuthRefreshResult(AuthRefreshMessage.ContentIsNull);
                if (IsNullOrWhiteSpace(result.AccessToken))
                    return new AuthRefreshResult(AuthRefreshMessage.AccessTokenNull);
                if (IsNullOrWhiteSpace(result.RefreshToken))
                    return new AuthRefreshResult(AuthRefreshMessage.RefreshTokenNull);

                await localStorage.SetItemAsync("accessToken", result.AccessToken);
                await localStorage.SetItemAsync("refreshToken", result.RefreshToken);

                return new AuthRefreshResult();
            }

            // Refactor to a separate Logout Service
            var revokeResponse = await httpClient.DeleteAsync("api/account/revoke");
            await localStorage.RemoveItemAsync("accessToken");
            await localStorage.RemoveItemAsync("refreshToken");
            // await authenticationStateProvider.GetAuthenticationStateAsync();
            await Console.Out.WriteLineAsync($"Revoke response: {revokeResponse}");
            return new AuthRefreshResult(AuthRefreshMessage.UnSuccessful);
        }
    }
}
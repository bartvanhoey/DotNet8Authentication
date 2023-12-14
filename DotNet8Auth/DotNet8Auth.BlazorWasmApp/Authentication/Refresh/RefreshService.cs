using System.Net.Http.Json;
using Blazored.LocalStorage;
using DotNet8Auth.Shared.Models.Authentication.Refresh;
using Microsoft.AspNetCore.Components.Authorization;
using static System.String;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Refresh
{
    public class RefreshService(
        IHttpClientFactory clientFactory,
        AuthenticationStateProvider authenticationStateProvider,
        ILocalStorageService localStorage
    ) : IRefreshService
    {
        private readonly HttpClient _httpClient = clientFactory.CreateClient("ServerAPI");

        public async Task<AuthRefreshResult> RefreshAsync()
        {
            // var accessToken = await localStorage.GetItemAsync<string>("accessToken");
            // var refreshToken = await localStorage.GetItemAsync<string>("refreshToken");
            //
            // var model = new RefreshInputModel(accessToken, refreshToken);
            // var response = await _httpClient.PostAsJsonAsync("api/account/refresh", model);
            //
            // if (response.IsSuccessStatusCode)
            // {
            //     // Refactor to a separate Logout Service
            //     var revokeResponse = await _httpClient.DeleteAsync("api/account/revoke");
            //     await localStorage.RemoveItemAsync(accessToken);
            //     await localStorage.RemoveItemAsync(refreshToken);
            //     await authenticationStateProvider.GetAuthenticationStateAsync();
            //     await Console.Out.WriteLineAsync($"Revoke response: {revokeResponse}");
            //     return new AuthRefreshResult(AuthRefreshMessage.UnSuccessful);
            // }
            //
            // var result = await response.Content.ReadFromJsonAsync<RefreshResult>();
            // if (result == null) return new AuthRefreshResult(AuthRefreshMessage.ContentIsNull);
            // if (IsNullOrWhiteSpace(result.AccessToken)) return new AuthRefreshResult(AuthRefreshMessage.AccessTokenNull);
            // if (IsNullOrWhiteSpace(result.RefreshToken)) return new AuthRefreshResult(AuthRefreshMessage.RefreshTokenNull);
            //
            // await localStorage.SetItemAsync("accessToken", result.AccessToken);
            // await localStorage.SetItemAsync("refreshToken", result.AccessToken);

            return new AuthRefreshResult();
        }
    }
}
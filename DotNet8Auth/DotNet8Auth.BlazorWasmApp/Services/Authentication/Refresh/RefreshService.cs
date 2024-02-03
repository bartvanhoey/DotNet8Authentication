using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Blazored.LocalStorage;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Logout;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Token;
using DotNet8Auth.Shared.Models.Authentication.Refresh;
using static System.String;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Refresh;

public class RefreshService(
    IHttpClientFactory clientFactory,
    IJwtTokenService jwtTokenService,
    ILogoutService? logoutService = null)
{
    public async Task<AuthRefreshResult> RefreshAsync()
    {
        // var accessToken = await localStorage.GetItemAsync<string>("accessToken");
        // var refreshToken = await localStorage.GetItemAsync<string>("refreshToken");

        var accessToken = await jwtTokenService.GetAccessTokenAsync();
        var refreshToken = await jwtTokenService.GetRefreshTokenAsync();


        var model = new RefreshInputModel(accessToken, refreshToken);

        var httpClient = clientFactory.CreateClient("ServerAPI");

        HttpResponseMessage? response = null;
        try
        {
            response = await httpClient.PostAsJsonAsync("api/account/refresh", model);
        }
        catch (Exception)
        {
            // TODO logging
        }

        if (response is { IsSuccessStatusCode: true })
        {
            var result = await response.Content.ReadFromJsonAsync<RefreshResult>();
            if (result == null) return new AuthRefreshResult(AuthRefreshMessage.ContentIsNull);
            if (IsNullOrWhiteSpace(result.AccessToken))
                return new AuthRefreshResult(AuthRefreshMessage.AccessTokenNull);
            if (IsNullOrWhiteSpace(result.RefreshToken))
                return new AuthRefreshResult(AuthRefreshMessage.RefreshTokenNull);

            // await localStorage.SetItemAsync("accessToken", result.AccessToken);
            // await localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            if (!jwtTokenService.IsAccessTokenValid(result.AccessToken))
                return new AuthRefreshResult(AuthRefreshMessage.AccessTokenInvalid);
            
            await jwtTokenService.SaveAccessTokenAsync(result.AccessToken);
            await jwtTokenService.SaveRefreshTokenAsync(result.RefreshToken);    
            
            return new AuthRefreshResult();



        }

        if (logoutService != null) await logoutService.LogoutAsync();

        return new AuthRefreshResult(AuthRefreshMessage.UnSuccessful);
    }
}
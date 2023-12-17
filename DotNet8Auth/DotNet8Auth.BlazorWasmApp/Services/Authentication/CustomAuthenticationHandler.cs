using System.Net;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Logout;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Refresh;
using static System.String;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication;

public class CustomAuthenticationHandler(
    IConfiguration configuration,
    ILocalStorageService localStorageService,    
    IHttpClientFactory clientFactory, ILogoutService logoutService
)
    : DelegatingHandler //AuthorizationMessageHandler   
{
    private bool _refreshing;
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await localStorageService.GetItemAsync<string>("accessToken", cancellationToken);
        var isToServer = request.RequestUri?.AbsoluteUri.StartsWith(configuration["ServerUrl"] ?? "") ?? false;

        if (isToServer && !IsNullOrEmpty(accessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        bool iShouldRefresh;
        HttpResponseMessage? response = null;
        try
        {
            response = await base.SendAsync(request, cancellationToken);
            iShouldRefresh = response.StatusCode == HttpStatusCode.Unauthorized;
            if (iShouldRefresh == false) return response;
        }
        catch (Exception e)
        {
            iShouldRefresh = true;
            Console.WriteLine(e);
        }

        if (_refreshing || IsNullOrEmpty(accessToken) || !iShouldRefresh) return response;

        try
        {
            _refreshing = true;
            var refreshService = new RefreshService(clientFactory, localStorageService, logoutService);
            var refreshResult = await refreshService.RefreshAsync();
            if (!refreshResult.Succeeded) return response;
                
            accessToken = await localStorageService.GetItemAsync<string>("accessToken", cancellationToken);
            
            if (isToServer && !IsNullOrEmpty(accessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken);

        }
        finally
        {
            _refreshing = false;
        }
    }
}
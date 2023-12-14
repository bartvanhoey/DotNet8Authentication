using System.Net;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using DotNet8Auth.BlazorWasmApp.Authentication.Refresh;

namespace DotNet8Auth.BlazorWasmApp
{
    public class CustomAuthorizationMessageHandler(
        IConfiguration configuration,
        ILocalStorageService localStorageService, IRefreshService refreshService)
        : DelegatingHandler //AuthorizationMessageHandler   
    {
        private bool _refreshing;
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await localStorageService.GetItemAsync<string>("accessToken", cancellationToken);
            var isToServer = request.RequestUri?.AbsoluteUri.StartsWith(configuration["ServerUrl"] ?? "") ?? false;

            if (isToServer && !string.IsNullOrEmpty(accessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response =  await base.SendAsync(request, cancellationToken);

            // if (!_refreshing && !string.IsNullOrEmpty(accessToken) && response.StatusCode == HttpStatusCode.Unauthorized)
            // {
            //     try
            //     {
            //         _refreshing = true;
            //
            //         var refreshResult = await refreshService.RefreshAsync();
            //         if (refreshResult.Succeeded)
            //         {
            //             accessToken = await localStorageService.GetItemAsync<string>("accessToken", cancellationToken);
            //
            //             if (isToServer && !string.IsNullOrEmpty(accessToken))
            //                 request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //
            //             response = await base.SendAsync(request, cancellationToken);
            //         }
            //     }
            //     finally
            //     {
            //         _refreshing = false;
            //     }
            // }
            return response;
        }
    }
}
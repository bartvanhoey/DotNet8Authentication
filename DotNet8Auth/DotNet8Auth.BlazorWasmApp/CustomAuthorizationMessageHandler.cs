using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DotNet8Auth.BlazorWasmApp
{
    public class CustomAuthorizationMessageHandler(
        IConfiguration configuration,
        ILocalStorageService localStorageService)
        : DelegatingHandler //AuthorizationMessageHandler   
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await localStorageService.GetItemAsync<string>("authToken", cancellationToken);
            var isToServer = request.RequestUri?.AbsoluteUri.StartsWith(configuration["ServerUrl"] ?? "") ?? false;

            if (isToServer && !string.IsNullOrEmpty(accessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
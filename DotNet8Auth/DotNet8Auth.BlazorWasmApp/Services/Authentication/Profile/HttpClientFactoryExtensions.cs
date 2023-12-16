using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Profile
{
    public static class HttpClientFactoryExtensions
    {
        public static async Task<HttpClient> CreateAuthHttpClient(this IHttpClientFactory clientFactory, ILocalStorageService localStorageService)
        {
            var http =  clientFactory.CreateClient("ServerAPI");
            var savedToken = await localStorageService.GetItemAsync<string>("accessToken");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);
            return http;
        }
    }
}
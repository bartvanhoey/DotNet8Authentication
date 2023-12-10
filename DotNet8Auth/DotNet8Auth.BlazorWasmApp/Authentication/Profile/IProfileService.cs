using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using DotNet8Auth.Shared.Models.Authentication.Profile;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Profile
{
    public interface IProfileService
    {
        public Task<ProfileResult> GetProfileAsync(string email);
    }

    public static class HttpClientFactoryExtensions
    {
        public static async Task<HttpClient> CreateClientWithAccessToken(this IHttpClientFactory clientFactory, ILocalStorageService localStorageService)
        {
            var http =  clientFactory.CreateClient("ServerAPI");
            var savedToken = await localStorageService.GetItemAsync<string>("authToken");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);
            return http;
        }
    }
    
    public class ProfileService(IHttpClientFactory clientFactory, ILocalStorageService localStorageService) : IProfileService
    {

        public async Task<ProfileResult> GetProfileAsync(string email)
        {
            var _http = await clientFactory.CreateClientWithAccessToken(localStorageService);
            
            var response = await _http.GetFromJsonAsync<ProfileResult>($"api/account/get-profile?email={email}");

            return response;
        }
    }

    public class ProfileResult
    {
        public string? UserName { get; set; } 
        public string? PhoneNumber { get; set; } 
    }
}
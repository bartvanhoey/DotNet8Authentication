using System.Net.Http.Headers;
using System.Net.Http.Json;
using DotNet8Auth.Shared.Models.Authentication.Profile;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Profile
{
    public interface IProfileService
    {
        public Task<ProfileResult> GetProfileAsync(string email);
    }

    public class ProfileService(IHttpClientFactory clientFactory) : IProfileService
    {
        private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");
        public async Task<ProfileResult> GetProfileAsync(string email)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYmFydHZhbmhvZXlAaG90bWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQ2NjI5OWM3LThlNWItNDI3MS1hZWQ0LTVhNmYxMzkxMmQ0ZSIsImp0aSI6IjMyYTdlYjY3LWM0MmQtNDcxNC1hYTcwLTQ0NTc1ZDNmOGZiYiIsImV4cCI6MTcwMjI0NzA5NiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzE5OSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcwMzYvIn0.7urR5L28vEgUA-G-bakwBSQ2uld9FbyglvRVPIxvI3Y");
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
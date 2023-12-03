using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DotNet8Auth.BlazorWebApp.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(IHttpClientFactory clientFactory, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorage)
        {
            _httpClient = clientFactory.CreateClient("ServerAPI");

            // if (_httpClient.BaseAddress == null)
            // {
            //     _httpClient.BaseAddress = new Uri("https://localhost:44370");
            // }
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

          public async Task<LoginResult> Login(InputModel loginModel)
    {
        var loginAsJson = JsonSerializer.Serialize(loginModel);
        var response = await _httpClient.PostAsync("api/auth/login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
        var jsonContent = await response.Content.ReadAsStringAsync();
        var loginResult = jsonContent.ConvertJsonTo<LoginResult>();

            
            
            
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadToken(loginResult.AccessToken) as JwtSecurityToken;

            var userId = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;


            if (!response.IsSuccessStatusCode)
        {
            loginResult.Successful = false;
            return loginResult;
        }
    
        Console.WriteLine();
        Console.WriteLine(loginResult.AccessToken);
        Console.WriteLine();


        await _localStorage.SetItemAsync("authToken", loginResult.AccessToken);
        ((PersistingRevalidatingAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email, userId);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.AccessToken);

        return loginResult;
    }
    }

}
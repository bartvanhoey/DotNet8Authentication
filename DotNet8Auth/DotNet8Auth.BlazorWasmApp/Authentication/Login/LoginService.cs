using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http.Json;
using DotNet8Auth.Shared.Models.Authentication.Login;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Login
{
    public class LoginService(
        IHttpClientFactory clientFactory,
        AuthenticationStateProvider authenticationStateProvider,
        ILocalStorageService localStorage)
        : ILoginService
    {
        private readonly HttpClient _httpClient = clientFactory.CreateClient("ServerAPI");

        public async Task<AuthLoginResult> Login(LoginInputModel input)
        {
            var response = await _httpClient.PostAsJsonAsync("api/account/login", input);
            var result = await response.Content.ReadFromJsonAsync<LoginResult>();

            if (result?.AccessToken == null) return new AuthLoginResult(AuthLoginMessage.AccessTokenNull); 

            if (response.IsSuccessStatusCode)
            {
                var jwtSecurityToken = new JwtSecurityTokenHandler().ReadToken(result.AccessToken) as JwtSecurityToken;
                var userId = jwtSecurityToken?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

                Console.WriteLine();
                Console.WriteLine($"AccessToken: {result.AccessToken}");
                Console.WriteLine($"UserId: {userId}");
                Console.WriteLine();
            
                await localStorage.SetItemAsync("authToken", result.AccessToken);
                ((CustomAuthenticationStateProvider)authenticationStateProvider).MarkUserAsAuthenticated(input.Email);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);
                return new AuthLoginResult();
            }

            await localStorage.RemoveItemAsync("authToken");

            return result.Status == "401"
                ? new AuthLoginResult(AuthLoginMessage.UnAuthorized)
                : new AuthLoginResult(AuthLoginMessage.Unknown);


        }
    }



}
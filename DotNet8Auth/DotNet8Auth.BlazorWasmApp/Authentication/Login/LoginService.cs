using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DotNet8Auth.BlazorWasmApp.Authentication;
using System.Net.Http.Json;
using DotNet8Auth.Shared.Models.Authentication.Login;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Login
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public LoginService(IHttpClientFactory clientFactory, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorage)
        {
            _httpClient = clientFactory.CreateClient("ServerAPI");
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<AuthLoginResult> Login(LoginInputModel loginModel)
        {
            var json = loginModel.ToJson();
            if (json == null) return new AuthLoginResult(AuthLoginMessage.LoginInputModelIsNull); ;
            var response = await _httpClient.PostAsync("api/account/login", new StringContent(json, Encoding.UTF8, "application/json"));

            var result = await response.Content.ReadFromJsonAsync<LoginResult>();
            if (result?.AccessToken == null) return new AuthLoginResult(AuthLoginMessage.AccessTokenNull); ;

            // var jsonContent = await response.Content.ReadAsStringAsync();
            // var loginResult = jsonContent.ConvertJsonTo<LoginResult>();

            if (response.IsSuccessStatusCode)
            {
                var jwtSecurityToken = new JwtSecurityTokenHandler().ReadToken(result.AccessToken) as JwtSecurityToken;
                var userId = jwtSecurityToken?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

                Console.WriteLine();
                Console.WriteLine(result.AccessToken);
                Console.WriteLine(userId);
                Console.WriteLine();

                await _localStorage.SetItemAsync("authToken", result.AccessToken);
                ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);
                return new AuthLoginResult();
            }
            else
            {
                await _localStorage.RemoveItemAsync("authToken");
                return result.Status == "401"
                    ? new AuthLoginResult(AuthLoginMessage.UnAuthorized)
                    : new AuthLoginResult(AuthLoginMessage.Unknown);
            }


        }
    }



}
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

        public async Task<AuthLoginResult> Login(InputModel loginModel)
        {
            var loginAsJson = JsonSerializer.Serialize(loginModel);
            var response = await _httpClient.PostAsync("api/account/login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
            var jsonContent = await response.Content.ReadAsStringAsync();
            var loginResult = jsonContent.ConvertJsonTo<LoginResult>();

            if (response.IsSuccessStatusCode)
            {
                var jwtSecurityToken = new JwtSecurityTokenHandler().ReadToken(loginResult.AccessToken) as JwtSecurityToken;
                var userId = jwtSecurityToken?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

                Console.WriteLine();
                Console.WriteLine(loginResult.AccessToken);
                Console.WriteLine(userId);
                Console.WriteLine();

                await _localStorage.SetItemAsync("authToken", loginResult.AccessToken);
                ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.AccessToken);
                return new AuthLoginResult() { Message = AuthLoginMessage.LoginSuccess };
            }
            else
            {
                await _localStorage.RemoveItemAsync("authToken");
                return loginResult.Status == 401
                    ? new AuthLoginResult() { Message = AuthLoginMessage.UnAuthorized }
                    : new AuthLoginResult() { Message = AuthLoginMessage.Unknown };
            }


        }
    }



}
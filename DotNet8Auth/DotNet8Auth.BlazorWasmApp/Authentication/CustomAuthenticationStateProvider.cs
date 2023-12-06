using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;



namespace DotNet8Auth.BlazorWasmApp.Authentication
{

    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IServiceProvider _services;
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        public CustomAuthenticationStateProvider(IHttpClientFactory clientFactory, IServiceProvider services, ILocalStorageService localStorageService)
        {
            _services = services;
            _localStorage = localStorageService;
            _httpClient = _httpClient = clientFactory.CreateClient("ServerAPI");

        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
                AuthenticationState? authenticationState = new(new ClaimsPrincipal(new ClaimsIdentity()));
            try
            {
                var savedToken = await _localStorage.GetItemAsync<string>("authToken");
                Console.WriteLine($"savedToken: {savedToken}");
                if (string.IsNullOrWhiteSpace(savedToken))
                {
                    NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
                    return authenticationState;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);
                authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt")));
                NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
                return authenticationState;
            }
            catch (Exception)
            {
                NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
                return authenticationState;
            }

        }

        public void MarkUserAsAuthenticated(string email)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            string payload = string.Empty;
            payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
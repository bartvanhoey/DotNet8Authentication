using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;


namespace DotNet8Auth.BlazorWasmApp.Authentication
{
    public class CustomAuthenticationStateProvider(
        ILocalStorageService localStorageService)
        : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            AuthenticationState authenticationState = new(new ClaimsPrincipal(new ClaimsIdentity()));
            try
            {
                var accessToken = await localStorageService.GetItemAsync<string>("accessToken");
                var isTokenValid = ValidateToken(accessToken);
                
                Console.WriteLine($"accessToken isValid: {isTokenValid}");
                
                if (string.IsNullOrWhiteSpace(accessToken) || !isTokenValid)
                {
                    await localStorageService.RemoveItemAsync("accessToken");
                    await localStorageService.RemoveItemAsync("refreshToken");

                    NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
                    return authenticationState;
                }

                authenticationState =
                    new AuthenticationState(
                        new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(accessToken), "jwt")));
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
            var authenticatedUser =
                new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth"));
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
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs == null) return claims;


            if (keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles))
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (roles != null)
                {
                    if ("[".StartsWith(roles.ToString()!.Trim()))
                    {
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);
                        if (parsedRoles == null) return claims;
                        claims.AddRange(parsedRoles.Select(parsedRole => new Claim(ClaimTypes.Role, parsedRole)));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
                    }

                    keyValuePairs.Remove(ClaimTypes.Role);
                }
            }

            claims.AddRange(keyValuePairs.Select(kvp =>
                new Claim(kvp.Key, kvp.Value.ToString() ?? throw new InvalidOperationException())));
            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }

            return Convert.FromBase64String(base64);
        }
        
        private static bool ValidateToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(accessToken, validationParameters, out validatedToken);
            return true;
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "Sample",
                ValidAudience = "Sample",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM")) // The same key as the one that generate the token
            };
        }

    }
}
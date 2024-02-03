using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.IdentityModel.Tokens;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Token;

public class JwtTokenService(ILocalStorageService localStorage, IConfiguration configuration) : IJwtTokenService
{
    private const string AccessToken = "accessToken";
    private const string RefreshToken = "refreshToken";

    public async Task SaveAccessTokenAsync(string accessToken) 
        => await localStorage.SetItemAsync(AccessToken, accessToken);
    
    public async Task RemoveAccessToken(string accessToken) 
        => await localStorage.SetItemAsync(AccessToken, accessToken);

    public async Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default)
        => await localStorage.GetItemAsync<string>(AccessToken, cancellationToken);

    public async Task RemoveAccessTokenAsync() 
        => await localStorage.RemoveItemAsync(AccessToken);

    public async Task SaveRefreshTokenAsync(string refreshToken)
        => await localStorage.SetItemAsync(RefreshToken, refreshToken);

    public async Task<string?> GetRefreshTokenAsync(CancellationToken cancellationToken = default)
        => await localStorage.GetItemAsync<string>(RefreshToken, cancellationToken);

    public async Task RemoveRefreshTokenAsync() 
        => await localStorage.RemoveItemAsync(RefreshToken);

    public  bool IsAccessTokenValid(string? accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken)) return false;
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters();

        // ReSharper disable once NotAccessedOutParameterVariable
        SecurityToken securityToken;
        try
        {
            // ReSharper disable once UnusedVariable
            IPrincipal principal = tokenHandler.ValidateToken(accessToken, validationParameters, out securityToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        return true;
    }

    public IEnumerable<Claim> GetClaims(string accessToken)
    {
        var claims = new List<Claim>();
        var payload = accessToken.Split('.')[1];
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


    private TokenValidationParameters GetValidationParameters() =>
        new()
        {
            ValidateLifetime = true, 
            ValidateAudience = true, 
            ValidateIssuer = true,  
            ValidIssuer = configuration["Jwt:ValidIssuer"],
            ValidAudience = configuration["Jwt:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM"))  // The same key as the one that generate the token
        };
}
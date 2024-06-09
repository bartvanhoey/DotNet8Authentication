using System.Security.Claims;

namespace Dotnet8Auth.BlazorServerApp.Services.Authentication.Token;

public interface IJwtTokenService
{
    Task SaveAccessTokenAsync(string accessToken);
    Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default);
    Task RemoveAccessTokenAsync(); 
    Task SaveRefreshTokenAsync(string refreshToken);
    Task<string?> GetRefreshTokenAsync(CancellationToken cancellationToken = default);
    Task RemoveRefreshTokenAsync();
    bool IsAccessTokenValid(string? accessToken);
    bool IsAccessTokenInValid(string? accessToken);
    IEnumerable<Claim> GetClaims(string accessToken);
}
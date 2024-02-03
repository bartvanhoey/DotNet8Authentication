using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Token;
using DotNet8Auth.Shared.Models.Authentication.Login;
using Microsoft.AspNetCore.Components.Authorization;
using static System.String;
using static DotNet8Auth.BlazorWasmApp.Services.Authentication.Login.AuthLoginMessage;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Login;

public class LoginService(
    IHttpClientFactory clientFactory,
    AuthenticationStateProvider authenticationStateProvider,
    IJwtTokenService jwtTokenService)
    : ILoginService
{
    private readonly HttpClient _httpClient = clientFactory.CreateClient("ServerAPI");

    public async Task<AuthLoginResult> Login(LoginInputModel input)
    {
        await jwtTokenService.RemoveAccessTokenAsync();

        HttpResponseMessage? response;
        LoginResult? result;
        try
        {
            response = await _httpClient.PostAsJsonAsync("api/account/login", input);
            result = await response.Content.ReadFromJsonAsync<LoginResult>();
            
            
        }
        catch (Exception)
        {
            return new AuthLoginResult(SomethingWentWrong);
        }

        if (result == null) return new AuthLoginResult(ContentIsNull);
        if (IsNullOrWhiteSpace(result.AccessToken)) return new AuthLoginResult(AccessTokenNull);
        if (IsNullOrWhiteSpace(result.RefreshToken)) return new AuthLoginResult(RefreshTokenNull);

        if (!response.IsSuccessStatusCode)
            return result.Status == "401"
                ? new AuthLoginResult(UnAuthorized)
                : new AuthLoginResult(Unknown);
            
        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadToken(result.AccessToken) as JwtSecurityToken;
        var userId = jwtSecurityToken?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        ((CustomAuthenticationStateProvider)authenticationStateProvider).MarkUserAsLoggedOut();
        
        Console.WriteLine();
        Console.WriteLine($"AccessToken: {result.AccessToken}");
        Console.WriteLine($"UserId: {userId}");
        Console.WriteLine();

        if (jwtTokenService.IsAccessTokenValid(result.AccessToken))
        {
            await jwtTokenService.SaveAccessTokenAsync(result.AccessToken);
            await jwtTokenService.SaveRefreshTokenAsync(result.RefreshToken);    
            ((CustomAuthenticationStateProvider)authenticationStateProvider).MarkUserAsAuthenticated(input.Email);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken); 
            return new AuthLoginResult();
        }

        return new AuthLoginResult(AccessTokenInvalid);




    }
}
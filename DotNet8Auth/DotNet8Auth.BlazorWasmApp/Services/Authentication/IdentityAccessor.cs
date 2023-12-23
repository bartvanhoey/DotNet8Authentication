using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Components.Authorization;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication;

public interface IIdentityAccessor
{
    Task<string?> GetUserNameAsync();
    Task<string?> GetUserIdAsync();
}

public class IdentityAccessor(AuthenticationStateProvider authenticationStateProvider) : IIdentityAccessor
{
    private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

    private async Task<IIdentity?> GetIdentityAsync()
        => (await _authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;

    private async Task<ClaimsPrincipal?> GetUserAsync()
        => (await _authenticationStateProvider.GetAuthenticationStateAsync()).User;

    public async Task<string?> GetUserNameAsync() => (await GetIdentityAsync())?.Name;

    public async Task<string?> GetUserIdAsync()=> (await GetUserAsync())?.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

}
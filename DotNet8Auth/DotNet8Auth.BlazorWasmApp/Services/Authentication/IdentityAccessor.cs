using System.Security.Principal;
using Microsoft.AspNetCore.Components.Authorization;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication
{
    public interface IIdentityAccessor
    {
        Task<string?> GetUserNameAsync();
    }

    public class IdentityAccessor(AuthenticationStateProvider authenticationStateProvider) : IIdentityAccessor
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

        private async Task<IIdentity?> GetIdentityAsync()
        => (await _authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;

        public async Task<string?> GetUserNameAsync() => (await GetIdentityAsync())?.Name;
    }
}
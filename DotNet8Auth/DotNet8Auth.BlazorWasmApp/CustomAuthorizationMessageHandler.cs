using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DotNet8Auth.BlazorWasmApp
{
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler   
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation) : base(provider, navigation)
        {
        }
    }
}
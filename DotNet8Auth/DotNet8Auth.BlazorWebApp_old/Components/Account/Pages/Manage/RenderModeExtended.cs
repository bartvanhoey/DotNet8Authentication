using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DotNet8Auth.BlazorWebApp.Components.Account.Pages.Manage
{
    public static class RenderModeExtended
    {
        public static IComponentRenderMode InteractiveServerWithoutPrerendering { get; } = 
            new InteractiveServerRenderMode(prerender: false);
    }
}
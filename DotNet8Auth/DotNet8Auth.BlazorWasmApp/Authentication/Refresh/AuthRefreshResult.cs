using DotNet8Auth.Shared.Extensions;
using static DotNet8Auth.BlazorWasmApp.Authentication.Refresh.AuthRefreshMessage;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Refresh
{
    public class AuthRefreshResult
    {
        public AuthRefreshResult() => Message = Successful;
        public AuthRefreshResult(AuthRefreshMessage message) => Message = message;
        public AuthRefreshMessage Message { get; set; }
        public bool Succeeded => Message == Successful;
    }
}
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Login;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Login
{
    public interface ILoginService
    {
        Task<AuthLoginResult> Login(LoginInputModel input);
    }

}

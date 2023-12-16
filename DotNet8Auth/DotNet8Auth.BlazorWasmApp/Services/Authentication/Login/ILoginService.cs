using DotNet8Auth.Shared.Models.Authentication.Login;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Login
{
    public interface ILoginService
    {
        Task<AuthLoginResult> Login(LoginInputModel input);
    }

}

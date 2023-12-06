using DotNet8Auth.Shared.Models.Authentication;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Login
{
    public interface ILoginService
    {
        Task<AuthLoginResult> Login(LoginInputModel inputModel);
    }

}

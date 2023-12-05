using DotNet8Auth.Shared.Models.Authentication;

namespace DotNet8Auth.BlazorWasmApp.Authentication
{
    public interface IAuthService
    {

        Task<LoginResult> Login(InputModel inputModel);
    }
}

using DotNet8Auth.Shared.Models.Authentication;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Register
{
    public interface IRegisterService
    {
        Task<AuthRegisterResult> RegisterAsync(RegisterInputModel inputModel);
    }

}
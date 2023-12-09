using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Register;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Register
{
    public interface IRegisterService
    {
        Task<AuthRegisterResult> RegisterAsync(RegisterInputModel inputModel);
    }

}
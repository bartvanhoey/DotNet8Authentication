using DotNet8Auth.Shared.Models.Authentication;

namespace DotNet8Auth.BlazorWebApp.Authentication
{
    public interface IAuthService
    {
        Task<LoginResult> Login(InputModel inputModel);
    }

}
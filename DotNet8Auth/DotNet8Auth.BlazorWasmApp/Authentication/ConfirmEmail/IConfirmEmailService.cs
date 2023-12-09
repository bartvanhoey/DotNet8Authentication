using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ConfirmEmail;

namespace DotNet8Auth.BlazorWasmApp.Authentication.ConfirmEmail
{
    public interface IConfirmEmailService
    {
        Task<AuthConfirmEmailResult> ConfirmEmailAsync(ConfirmEmailInputModel input);
    }
}
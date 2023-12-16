using DotNet8Auth.Shared.Models.Authentication.ResendEmailConfirmation;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ResendEmailConfirmation
{
    public interface IResendEmailConfirmationService
    {
        Task<AuthResendEmailConfirmationResult> ResendEmailConfirmationAsync(ResendEmailConfirmationInputModel input);
    }
}
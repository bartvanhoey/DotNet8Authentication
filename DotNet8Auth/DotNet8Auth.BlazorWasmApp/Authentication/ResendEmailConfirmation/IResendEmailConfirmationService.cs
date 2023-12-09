using DotNet8Auth.Shared.Models.Authentication.ResendEmailConfirmation;

namespace DotNet8Auth.BlazorWasmApp.Authentication.ResendEmailConfirmation
{
    public interface IResendEmailConfirmationService
    {
        Task<AuthResendEmailConfirmationResult> ResendEmailConfirmationAsync(ResendEmailConfirmationInputModel input);
    }

    public class ResendEmailConfirmationService : IResendEmailConfirmationService
    {
        public Task<AuthResendEmailConfirmationResult> ResendEmailConfirmationAsync(ResendEmailConfirmationInputModel input)
        {
            throw new NotImplementedException();
        }
    }

    public class AuthResendEmailConfirmationResult
    {
    }
}
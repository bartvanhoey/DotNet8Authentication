using DotNet8Auth.Shared.Models.Authentication.ForgotPassword;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ForgotPassword
{
    public interface IForgotPasswordService
    {
        Task<AuthForgotPasswordResult> AskPasswordResetAsync(ForgotPasswordInputModel input);
    }
}
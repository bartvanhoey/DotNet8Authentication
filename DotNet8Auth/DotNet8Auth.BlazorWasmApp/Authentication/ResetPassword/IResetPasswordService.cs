using DotNet8Auth.Shared.Models.Authentication.ResetPassword;

namespace DotNet8Auth.BlazorWasmApp.Authentication.ResetPassword
{
    public interface IResetPasswordService
    {
        Task<AuthResetPasswordResult> ResetPasswordAsync(ResetPasswordInputModel input);
    }
}
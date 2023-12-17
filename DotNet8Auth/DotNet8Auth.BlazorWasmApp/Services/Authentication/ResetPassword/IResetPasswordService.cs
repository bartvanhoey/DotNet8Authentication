using DotNet8Auth.Shared.Models.Authentication.ResetPassword;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ResetPassword;

public interface IResetPasswordService
{
    Task<AuthResetPasswordResult> ResetPasswordAsync(ResetPasswordInputModel input);
}
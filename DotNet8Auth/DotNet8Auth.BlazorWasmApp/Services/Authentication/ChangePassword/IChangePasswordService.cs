using DotNet8Auth.Shared.Models.Authentication.ChangePassword;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangePassword;

public interface IChangePasswordService
{
    Task<AuthChangePasswordResult> ChangePasswordAsync(ChangePasswordInputModel input);
}
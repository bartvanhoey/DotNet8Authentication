namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangePassword;

public interface IUserHasPasswordService
{
    Task<AuthUserHasPasswordResult> UserHasPasswordAsync();
}
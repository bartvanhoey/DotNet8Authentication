namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Refresh;

public interface IRefreshService
{
    Task<AuthRefreshResult> RefreshAsync();
}
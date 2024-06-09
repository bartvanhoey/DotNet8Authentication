namespace Dotnet8Auth.BlazorServerApp.Services.Authentication.Refresh;

public interface IRefreshService
{
    Task<AuthRefreshResult> RefreshAsync();
}
namespace DotNet8Auth.BlazorWasmApp.Authentication.Refresh
{
    public interface IRefreshService
    {
        Task<AuthRefreshResult> RefreshAsync();
    }
}
using System.Net.Http.Json;
using DotNet8Auth.BlazorWasmApp.Services.Logging;
using DotNet8Auth.Shared.Models.Authentication.ChangePassword;
using static DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangePassword.AuthUserHasPasswordMessage;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangePassword;

public class UserHasPasswordService(IHttpClientFactory clientFactory, ISerilogService serilogService) : IUserHasPasswordService
{
    private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");

    public async Task<AuthUserHasPasswordResult> UserHasPasswordAsync()
    {
        try
        {
            var response = await _http.GetFromJsonAsync<UserHasPasswordResult>("api/account/user-has-password");
            if (response != null && response.Succeeded) return new AuthUserHasPasswordResult(response.UserHasPassword);
            await serilogService.LogError(response?.Message ?? "something went wrong", nameof(UserHasPasswordAsync));
            return new AuthUserHasPasswordResult(SomethingWentWrong);
        }
        catch (Exception exception)
        {
            await serilogService.LogError(exception, nameof(UserHasPasswordAsync));
            return new AuthUserHasPasswordResult(SomethingWentWrong);
        }
    }
}
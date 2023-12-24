using System.Net.Http.Json;
using DotNet8Auth.BlazorWasmApp.Services.Logging;
using DotNet8Auth.Shared.Models.Authentication.ChangeEmail;
using static System.ArgumentNullException;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangeEmail;

public class ChangeEmailService(IHttpClientFactory clientFactory, ISerilogService serilogService) : IChangeEmailService
{
    private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");

    public async Task<AuthChangeEmailResult> ChangeEmailAsync(string newEmail)
    {
        try
        {
            ThrowIfNull(newEmail);
            var model = new ChangeEmailInputModel(newEmail);
            var response = await _http.PostAsJsonAsync("api/account/change-email", model);
            var result = await response.Content.ReadFromJsonAsync<ChangeEmailResult>();
            if (result is { Succeeded: true }) return new AuthChangeEmailResult();
            await serilogService.LogError(result?.Message ?? "Change email went wrong", nameof(ChangeEmailAsync));
            return new AuthChangeEmailResult(AuthChangeEmailInfo.SomethingWentWrong);
        }
        catch (Exception exception)
        {
            await serilogService.LogError(exception, nameof(ChangeEmailAsync));
            return new AuthChangeEmailResult(AuthChangeEmailInfo.SomethingWentWrong);
        }
    }

    public async Task<AuthConfirmChangeEmailResult> ConfirmChangeEmailAsync(string email, string newEmail, string code)
    {
        try
        {
            ThrowIfNull(email);
            ThrowIfNull(newEmail);
            ThrowIfNull(code);
            var model = new ConfirmChangeEmailInputModel(email, newEmail, code);
            var response = await _http.PostAsJsonAsync("api/account/confirm-change-email", model);
            var result = await response.Content.ReadFromJsonAsync<ConfirmChangeEmailResult>();
            if (result is { Succeeded: true }) return new AuthConfirmChangeEmailResult();
            await serilogService.LogError(result?.Message ?? "Confirm change email went wrong",
                nameof(ConfirmChangeEmailAsync));
            return new AuthConfirmChangeEmailResult(AuthConfirmChangeEmailInfo.SomethingWentWrong);
        }
        catch (Exception exception)
        {
            await serilogService.LogError(exception, nameof(ChangeEmailAsync));
            return new AuthConfirmChangeEmailResult(AuthConfirmChangeEmailInfo.SomethingWentWrong);
        }
    }

    public async Task<AuthIsEmailConfirmedResult> IsEmailConfirmedAsync()
    {
        try
        {
            var response = await _http.GetFromJsonAsync<ChangeEmailConfirmedResult>("api/account/is-email-confirmed");
            if (response is { Succeeded: true })
                return new AuthIsEmailConfirmedResult(response.IsEmailConfirmed);
            await serilogService.LogError(response?.Message ?? "something went wrong", nameof(IsEmailConfirmedAsync));
            return new AuthIsEmailConfirmedResult(AuthIsEmailConfirmedMessage.SomethingWentWrong);
        }
        catch (Exception exception)
        {
            await serilogService.LogError(exception, nameof(IsEmailConfirmedAsync));
            return new AuthIsEmailConfirmedResult(AuthIsEmailConfirmedMessage.SomethingWentWrong);
        }
    }
}
using System.Net.Http.Json;
using DotNet8Auth.BlazorWasmApp.Services.Logging;
using DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

using DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangeEmail;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangeEmail
{
    public class ChangeEmailService(IHttpClientFactory clientFactory, ISerilogService serilogService) : IChangeEmailService
    {

        private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");

        public async Task<AuthChangeEmailResult> ChangeEmailAsync(string newEmail)
        {
            try
            {
                var model = new ChangeEmailInputModel() { NewEmail = newEmail };
                var response = await _http.PostAsJsonAsync("api/account/change-email", model);
                var result = await response.Content.ReadFromJsonAsync<ChangeEmailResult>();
                if (result != null && result.Succeeded) return new AuthChangeEmailResult();
                await serilogService.LogError(result?.Message ?? "Change email went wrong", nameof(ChangeEmailAsync));
                return new AuthChangeEmailResult(AuthChangeEmailInfo.SomethingWentWrong);

            }
            catch (Exception exception)
            {
                await serilogService.LogError(exception, nameof(ChangeEmailAsync));
                return new AuthChangeEmailResult(AuthChangeEmailInfo.SomethingWentWrong);
            }
        }

        public async Task<AuthConfirmChangeEmailResult> ConfirmChangeEmailAsync(string newEmail, string code)
        {
            try
            {
                var model = new ConfirmChangeEmailInputModel() { NewEmail = newEmail, Code = code };
                var response = await _http.PostAsJsonAsync("api/account/confirm-change-email", model);
                var result = await response.Content.ReadFromJsonAsync<ConfirmChangeEmailResult>();
                if (result != null && result.Succeeded) return new AuthConfirmChangeEmailResult();
                await serilogService.LogError(result?.Message ?? "Confirm change email went wrong", nameof(ConfirmChangeEmailAsync));
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
                if (response != null && response.Succeeded) return new AuthIsEmailConfirmedResult(response.IsEmailConfirmed);
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

    public class ConfirmChangeEmailResult
    {
        public string? Message { get; set; }
        public bool Succeeded => Status == "Success";
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string? Status { get; set; }
    }





    public class AuthConfirmChangeEmailResult
    {

        public AuthConfirmChangeEmailResult()
        {
            Message = AuthConfirmChangeEmailInfo.Success;
            IsEmailChanged = true;
        }

        public AuthConfirmChangeEmailResult(bool isEmailChanged)
        {
            Message = AuthConfirmChangeEmailInfo.Success;
            IsEmailChanged = isEmailChanged;
        }

        public AuthConfirmChangeEmailResult(AuthConfirmChangeEmailInfo message)
            => Message = message;

        public bool Succeeded => Message == AuthConfirmChangeEmailInfo.Success;
        public AuthConfirmChangeEmailInfo Message { get; set; }

        public bool IsEmailChanged { get; set; }
    }


    public enum AuthConfirmChangeEmailInfo
    {
        Success = 0,
        SomethingWentWrong = 2,
    }
}
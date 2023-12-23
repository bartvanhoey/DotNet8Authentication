using System.Net.Http.Json;
using DotNet8Auth.BlazorWasmApp.Services.Logging;
using DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

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
    public enum AuthChangeEmailInfo
    {
        Successful = 0,
        SomethingWentWrong = 2,
    }
}
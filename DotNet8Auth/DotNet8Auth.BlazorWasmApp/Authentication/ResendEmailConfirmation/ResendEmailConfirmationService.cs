using System.Net.Http.Json;
using DotNet8Auth.Shared.Models.Authentication.ConfirmEmail;
using DotNet8Auth.Shared.Models.Authentication.ResendEmailConfirmation;

namespace DotNet8Auth.BlazorWasmApp.Authentication.ResendEmailConfirmation
{
    public class ResendEmailConfirmationService(IHttpClientFactory clientFactory) : IResendEmailConfirmationService
    {
        private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");
        public async Task<AuthResendEmailConfirmationResult> ResendEmailConfirmationAsync(
            ResendEmailConfirmationInputModel input)
        {
            var response = await _http.PostAsJsonAsync("api/account/resend-email-confirmation", input);
            var result = await response.Content.ReadFromJsonAsync<ConfirmEmailResult>();

            return result is { Succeeded: true }
                ? new AuthResendEmailConfirmationResult()
                : new AuthResendEmailConfirmationResult(AuthResendConfirmEmailConfirmationInfo
                    .ResendEmailConfirmationUnsuccessful);
        }
    }
}
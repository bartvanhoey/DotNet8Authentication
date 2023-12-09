using System.Net.Http.Json;
using DotNet8Auth.Shared.Models.Authentication.ConfirmEmail;

namespace DotNet8Auth.BlazorWasmApp.Authentication.ConfirmEmail
{
    public class ConfirmEmailService(IHttpClientFactory clientFactory) : IConfirmEmailService
    {
        private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");

        public async Task<AuthConfirmEmailResult> ConfirmEmailAsync(ConfirmEmailInputModel input)
        {
            var response = await _http.PostAsJsonAsync("api/account/confirm-email",input);
            var result = await response.Content.ReadFromJsonAsync<ConfirmEmailResult>();

            return result is { Succeeded: true }
                ? new AuthConfirmEmailResult()
                : new AuthConfirmEmailResult(AuthConfirmEmailInfo.ConfirmEmailUnsuccessful);
        }
    }
}
using System.Net.Http.Json;
using System.Text;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;

namespace DotNet8Auth.BlazorWasmApp.Authentication.ConfirmEmail
{
    public class ConfirmEmailService : IConfirmEmailService
    {
        private readonly HttpClient _http;

        public ConfirmEmailService(IHttpClientFactory clientFactory) 
            => _http = clientFactory.CreateClient("ServerAPI");

        public async Task<AuthConfirmEmailResult> ConfirmEmailAsync(ConfirmEmailInputModel input)
        {
            var json = input.ToJson();
            if (json == null) return new AuthConfirmEmailResult(AuthConfirmEmailInfo.ConfirmEmailUnsuccessful);
            var response = await _http.PostAsync("api/account/confirm-email", new StringContent(json, Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadFromJsonAsync<ConfirmEmailResult>();

            return result != null && result.Succeeded
                ? new AuthConfirmEmailResult()
                : new AuthConfirmEmailResult(AuthConfirmEmailInfo.ConfirmEmailUnsuccessful);
            
        }
    }

    public class ConfirmEmailModel{
        public string UserId { get; set; } = "";
        public string Code { get; set; } = "";
    }

}
using System.Net.Http.Json;
using DotNet8Auth.BlazorWasmApp.Authentication.Register;
using DotNet8Auth.Shared.Models.Authentication.ForgotPassword;

namespace DotNet8Auth.BlazorWasmApp.Authentication.ForgotPassword
{
    public class ForgotPasswordService(IHttpClientFactory clientFactory) : IForgotPasswordService
    {
        private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");

        public async Task<AuthForgotPasswordResult> AskPasswordResetAsync(ForgotPasswordInputModel input)
        {
            var response = await _http.PostAsJsonAsync("api/account/forgot-password", input);
            var result = await response.Content.ReadFromJsonAsync<RegisterResult>();

            return result is { Succeeded: true }
                ? new AuthForgotPasswordResult()
                : new AuthForgotPasswordResult(AuthForgotPasswordInfo.UnSuccessful);
        }
    }
}
using System.Net.Http.Json;
using DotNet8Auth.BlazorWasmApp.Authentication.Register;
using DotNet8Auth.Shared.Models.Authentication.ResetPassword;

namespace DotNet8Auth.BlazorWasmApp.Authentication.ResetPassword
{
    public class ResetPasswordService(IHttpClientFactory clientFactory) : IResetPasswordService
    {
        private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");

        public async Task<AuthResetPasswordResult> ResetPasswordAsync(ResetPasswordInputModel input)
        {
            var response = await _http.PostAsJsonAsync("api/account/reset-password", input);
            var result = await response.Content.ReadFromJsonAsync<ResetPasswordResult>();

            return result is { Succeeded: true }
                ? new AuthResetPasswordResult()
                : new AuthResetPasswordResult(AuthResetPasswordInfo.ResetPasswordUnSuccessful);
        }
    }
}
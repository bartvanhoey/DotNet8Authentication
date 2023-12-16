using System.Net.Http.Json;
using DotNet8Auth.BlazorWasmApp.Authentication.Register;
using DotNet8Auth.Shared.Models.Authentication.ForgotPassword;
using static DotNet8Auth.BlazorWasmApp.Authentication.ForgotPassword.AuthForgotPasswordInfo;

namespace DotNet8Auth.BlazorWasmApp.Authentication.ForgotPassword
{
    public class ForgotPasswordService(IHttpClientFactory clientFactory) : IForgotPasswordService
    {
        private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");

        public async Task<AuthForgotPasswordResult> AskPasswordResetAsync(ForgotPasswordInputModel input)
        {
            RegisterResult? result;
            try
            {
                var response = await _http.PostAsJsonAsync("api/account/forgot-password", input);
                result = await response.Content.ReadFromJsonAsync<RegisterResult>();
            }
            catch (Exception)
            {
                // TODO logging
                return new AuthForgotPasswordResult(SomethingWentWrong);
            }

            return result is { Succeeded: true }
                ? new AuthForgotPasswordResult()
                : new AuthForgotPasswordResult(UnSuccessful);
        }
    }
}
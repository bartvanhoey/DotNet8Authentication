using System.Net.Http.Json;
using DotNet8Auth.Shared.Models.Authentication.Register;
using static DotNet8Auth.BlazorWasmApp.Services.Authentication.Register.AuthRegisterInfo;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Register
{
    public class RegisterService(IHttpClientFactory clientFactory) : IRegisterService
    {
        private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");

        public async Task<AuthRegisterResult> RegisterAsync(RegisterInputModel input)
        {
            RegisterResult? result;
            try
            {
                var response = await _http.PostAsJsonAsync("api/account/register", input);
                result = await response.Content.ReadFromJsonAsync<RegisterResult>();
            }
            catch (Exception)
            {
                return new AuthRegisterResult(SomethingWentWrong);
            }

            return result is { Succeeded: true }
                ? new AuthRegisterResult()
                : new AuthRegisterResult(RegistrationUnsuccessful);
        }
    }
}
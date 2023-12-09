using System.Net.Http.Json;
using DotNet8Auth.Shared.Models.Authentication.Register;
using static DotNet8Auth.BlazorWasmApp.Authentication.Register.AuthRegisterInfo;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Register
{
    public class RegisterService(IHttpClientFactory clientFactory) : IRegisterService
    {
        private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");

        public async Task<AuthRegisterResult> RegisterAsync(RegisterInputModel input)
        {
            var response = await _http.PostAsJsonAsync("api/account/register", input);
            var result = await response.Content.ReadFromJsonAsync<RegisterResult>();

            return result is { Succeeded: true }
                ? new AuthRegisterResult()
                : new AuthRegisterResult(RegistrationUnsuccessful);
        }
    }
}
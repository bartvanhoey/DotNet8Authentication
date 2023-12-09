using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Register;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly HttpClient _http;


        public RegisterService(IHttpClientFactory clientFactory)
            => _http = clientFactory.CreateClient("ServerAPI");
        public async Task<AuthRegisterResult> RegisterAsync(RegisterInputModel input)
        {
            // var registerAsJson = JsonSerializer.Serialize(input);
            var json = input.ToJson();
            if (json == null) return new AuthRegisterResult(AuthRegisterInfo.RegistrationUnsuccessful);

            var response = await _http.PostAsync("api/account/register",
                new StringContent(json, Encoding.UTF8, "application/json"));

            var result = await response.Content.ReadFromJsonAsync<RegisterResult>();


            return result != null && result.Succeeded
                ? new AuthRegisterResult()
                : new AuthRegisterResult(AuthRegisterInfo.RegistrationUnsuccessful);
        }
    }
}
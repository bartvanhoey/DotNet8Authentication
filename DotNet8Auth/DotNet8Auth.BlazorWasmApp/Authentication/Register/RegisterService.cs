using System.Text;
using System.Text.Json;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly HttpClient _httpClient;


        public RegisterService(IHttpClientFactory clientFactory) => _httpClient = clientFactory.CreateClient("ServerAPI");
        public async Task<AuthRegisterResult> Register(RegisterInputModel inputModel)
        {
          var registerAsJson = JsonSerializer.Serialize(inputModel);
            var response = await _httpClient.PostAsync("api/account/register", new StringContent(registerAsJson, Encoding.UTF8, "application/json"));
            var jsonContent = await response.Content.ReadAsStringAsync();
            var result = jsonContent.ConvertJsonTo<RegisterResult>();
            return result.Succeeded ? new AuthRegisterResult() : new AuthRegisterResult(AuthRegisterInfo.RegistrationUnsuccessful);
        }
    }
}
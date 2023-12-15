using System.Net.Http.Json;
using DotNet8Auth.Shared.Models.Authentication.Profile;
using static DotNet8Auth.BlazorWasmApp.Authentication.Profile.AuthSetPhoneNumberInfo;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Profile
{
    public class ProfileService(IHttpClientFactory clientFactory) : IProfileService
    {
        private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");
        public async Task<ProfileResult?> GetProfileAsync(string email)
        {
            var response = await _http.GetFromJsonAsync<ProfileResult>($"api/account/get-profile?email={email}");
            return response;
        }

        public async Task<AuthSetPhoneNumberResult> SetPhoneNumberAsync(SetPhoneNumberInputModel model)
        {
            var response = await _http.PostAsJsonAsync("api/account/set-phone-number", model);
            var result = await response.Content.ReadFromJsonAsync<ProfileResult>();

            return result is { Succeeded: true }
                ? new AuthSetPhoneNumberResult()
                : new AuthSetPhoneNumberResult(SetPhoneNumberUnsuccessful);
            
        }
    }

     
}
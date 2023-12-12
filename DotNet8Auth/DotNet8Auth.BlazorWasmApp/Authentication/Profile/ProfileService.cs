using System.Net.Http.Json;
using Blazored.LocalStorage;
using DotNet8Auth.Shared.Models.Authentication.Profile;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Profile
{
    public class ProfileService(IHttpClientFactory clientFactory, ILocalStorageService localStorage) : IProfileService
    {

        public async Task<ProfileResult?> GetProfileAsync(string email)
        {
            var http = await clientFactory.CreateAuthHttpClient(localStorage);
            var response = await http.GetFromJsonAsync<ProfileResult>($"api/account/get-profile?email={email}");
            return response;
        }

        public async Task<AuthSetPhoneNumberResult> SetPhoneNumberAsync(SetPhoneNumberInputModel model)
        {
            var http = await clientFactory.CreateAuthHttpClient(localStorage);
            var response = await http.PostAsJsonAsync("api/account/set-phone-number", model);
            var result = await response.Content.ReadFromJsonAsync<ProfileResult>();

            return result is { Succeeded: true }
                ? new AuthSetPhoneNumberResult()
                : new AuthSetPhoneNumberResult(AuthSetPhoneNumberInfo.SetPhoneNumberUnsuccessful);
            
        }
    }
}
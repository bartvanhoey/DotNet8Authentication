using System.Net.Http.Json;
using DotNet8Auth.Shared.Models.Authentication.SetPhoneNumber;
using static DotNet8Auth.BlazorWasmApp.Services.Authentication.Profile.AuthSetPhoneNumberInfo;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Profile;

public class ProfileService(IHttpClientFactory clientFactory) : IProfileService
{
    private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");
    public async Task<ProfileResult?> GetProfileAsync(string email)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<ProfileResult>($"api/account/get-profile?email={email}");
            return response;
        }
        catch (Exception)
        {
            // TODO logging
            return default;
        }
    }

    public async Task<AuthSetPhoneNumberResult> SetPhoneNumberAsync(SetPhoneNumberInputModel model)
    {
        ProfileResult? result;
        try
        {
            var response = await _http.PostAsJsonAsync("api/account/set-phone-number", model);
            result = await response.Content.ReadFromJsonAsync<ProfileResult>();
        }
        catch (Exception)
        {
            // TODO logging
            return new AuthSetPhoneNumberResult(SomethingWentWrong);
        }

        return result is { Succeeded: true }
            ? new AuthSetPhoneNumberResult()
            : new AuthSetPhoneNumberResult(SetPhoneNumberUnsuccessful);
            
    }
}
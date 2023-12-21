using DotNet8Auth.Shared.Models.Authentication.SetPhoneNumber;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Profile;

public interface IProfileService
{
    public Task<ProfileResult?> GetProfileAsync();
    Task<AuthSetPhoneNumberResult> SetPhoneNumberAsync(SetPhoneNumberInputModel model);
}
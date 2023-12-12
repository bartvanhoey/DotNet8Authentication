using DotNet8Auth.Shared.Models.Authentication.Profile;

namespace DotNet8Auth.BlazorWasmApp.Authentication.Profile
{
    public interface IProfileService
    {
        public Task<ProfileResult?> GetProfileAsync(string email);
        Task<AuthSetPhoneNumberResult> SetPhoneNumberAsync(SetPhoneNumberInputModel model);
    }
}
using static DotNet8Auth.BlazorWasmApp.Services.Authentication.Profile.AuthSetPhoneNumberInfo;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Profile;

public class AuthSetPhoneNumberResult(AuthSetPhoneNumberInfo message) 
{
    public AuthSetPhoneNumberResult() : this(SetPhoneNumberSuccessful)
    {
    }
    public bool Succeeded => Message == SetPhoneNumberSuccessful;
    private AuthSetPhoneNumberInfo Message { get; set; } = message;
}
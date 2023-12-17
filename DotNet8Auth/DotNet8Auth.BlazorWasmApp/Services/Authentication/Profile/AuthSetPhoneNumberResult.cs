namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Profile;

public class AuthSetPhoneNumberResult(AuthSetPhoneNumberInfo message) 
{
    public AuthSetPhoneNumberResult() : this(AuthSetPhoneNumberInfo.SetPhoneNumberSuccessful)
    {
    }
    public bool Succeeded => Message == AuthSetPhoneNumberInfo.SetPhoneNumberSuccessful;
    private AuthSetPhoneNumberInfo Message { get; set; } = message;
}
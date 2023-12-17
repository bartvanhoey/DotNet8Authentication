using static DotNet8Auth.BlazorWasmApp.Services.Authentication.Register.AuthRegisterInfo;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Register;

public class AuthRegisterResult(AuthRegisterInfo message)
{
    public AuthRegisterResult() : this(RegistrationSuccessful)
    {
    }

    public bool Succeeded => Message == RegistrationSuccessful;
    private AuthRegisterInfo Message { get; set; } = message;
}
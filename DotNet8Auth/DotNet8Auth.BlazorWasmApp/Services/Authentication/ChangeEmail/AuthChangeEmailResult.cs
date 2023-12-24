namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangeEmail;

public class AuthChangeEmailResult(AuthChangeEmailInfo message)
{
    public AuthChangeEmailResult() : this(AuthChangeEmailInfo.Successful)
    {
    }

    public bool Succeeded => Message == AuthChangeEmailInfo.Successful;
    public AuthChangeEmailInfo Message { get; } = message;

}
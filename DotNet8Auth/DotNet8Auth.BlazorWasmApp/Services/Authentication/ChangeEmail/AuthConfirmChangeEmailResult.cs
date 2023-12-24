namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangeEmail;

public class AuthConfirmChangeEmailResult
{
    public AuthConfirmChangeEmailResult()
    {
        Message = AuthConfirmChangeEmailInfo.Success;
        IsEmailChanged = true;
    }

    public AuthConfirmChangeEmailResult(bool isEmailChanged)
    {
        Message = AuthConfirmChangeEmailInfo.Success;
        IsEmailChanged = isEmailChanged;
    }

    public AuthConfirmChangeEmailResult(AuthConfirmChangeEmailInfo message)
        => Message = message;

    public bool Succeeded => Message == AuthConfirmChangeEmailInfo.Success;
    public AuthConfirmChangeEmailInfo Message { get; set; }

    public bool IsEmailChanged { get; set; }
}
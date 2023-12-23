namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangePassword;

public class AuthUserHasPasswordResult
{
    public AuthUserHasPasswordResult(bool userHasPassword)
    {
        Message = AuthUserHasPasswordMessage.Success;
        UserHasPassword = userHasPassword;
    }

    public AuthUserHasPasswordResult(AuthUserHasPasswordMessage message) => Message = message;

    public bool Succeeded => Message == AuthUserHasPasswordMessage.Success;
    public AuthUserHasPasswordMessage Message { get; set; }

    public bool UserHasPassword { get; set; }

}
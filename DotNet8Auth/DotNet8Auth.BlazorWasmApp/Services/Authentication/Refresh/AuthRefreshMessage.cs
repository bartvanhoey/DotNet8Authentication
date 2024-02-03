namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Refresh;

public enum AuthRefreshMessage
{
    Successful = 0,
    UnSuccessful = 1,
    ContentIsNull = 2,
    AccessTokenNull = 3,
    RefreshTokenNull = 4,
    AccessTokenInvalid = 5,
}
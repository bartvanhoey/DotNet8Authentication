namespace DotNet8Auth.BlazorWasmApp.Authentication.Login
{
    public enum AuthLoginMessage { 
        LoginSuccess = 0, 
        UnAuthorized = 1,
        Unknown = 2,
        AccessTokenNull = 3,
        RefreshTokenNull = 3,
        ContentIsNull = 4,
        SomethingWentWrong = 5,
    }

}

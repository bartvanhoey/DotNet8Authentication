namespace Dotnet8Auth.BlazorServerApp.HttpClients;

public class AuthResponse
{
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
}
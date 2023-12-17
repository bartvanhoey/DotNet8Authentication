namespace DotNet8Auth.Shared.Models.Authentication.Login;

public class LoginResult
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime ValidTo { get; set; }
    public bool Successful{ get; set; }

    public string? Type { get; set; }

    public string? Title { get; set; }
       
    public string? Status { get; set; }
        
    public string? TraceId { get; set; }
    public string? Message { get; set; }

}
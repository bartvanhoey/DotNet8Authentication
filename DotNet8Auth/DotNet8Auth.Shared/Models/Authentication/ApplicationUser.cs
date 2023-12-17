using Microsoft.AspNetCore.Identity;

namespace DotNet8Auth.Shared.Models.Authentication;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; } 
    public DateTime RefreshTokenExpiry { get; set; } 

}
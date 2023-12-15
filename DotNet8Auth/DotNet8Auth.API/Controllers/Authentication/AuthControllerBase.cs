using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DotNet8Auth.API.Controllers.Authentication
{
    public class AuthControllerBase(UserManager<ApplicationUser> userManager) : ControllerBase
    {
        protected async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user, string jwtValidIssuer, string jwtValidAudience, string jwtSecurityKey)
        {
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Email ?? throw new InvalidOperationException()),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await userManager.GetRolesAsync(user);
            if (userRoles is { Count: > 0 })
            {
                authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
            }

            var token = new JwtSecurityToken(
                issuer: jwtValidIssuer,
                audience: jwtValidAudience,
                expires: DateTime.UtcNow.AddSeconds(60),
                claims: authClaims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecurityKey)),
                    SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static System.DateTime;
using static System.Guid;
using static System.Security.Claims.ClaimTypes;
using static System.String;
using static System.Text.Encoding;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Microsoft.IdentityModel.Tokens.SecurityAlgorithms;
using Convert = System.Convert;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [Route("api/Account")]
    [ApiController]
    public class LoginController(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
        : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginInputModel? input)
        {
            try
            {
                if (input is null)
                    return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Login went wrong"));

                var user = await userManager.FindByEmailAsync(input.Email);
                if (user == null)
                    return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Invalid Password or Username"));

                if (IsNullOrWhiteSpace(user.Email))
                    return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Email Empty"));

                var isPasswordValid = await userManager.CheckPasswordAsync(user, input.Password);
                if (!isPasswordValid)
                    return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Invalid Password or Username"));
                
                var validIssuer = configuration["Jwt:ValidIssuer"];
                if (IsNullOrEmpty(validIssuer)) 
                    return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Invalid Issuer"));

                var validAudience = configuration["Jwt:ValidAudience"];
                if (IsNullOrEmpty(validAudience)) 
                    return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Invalid Audience"));

                var securityKey = configuration["Jwt:SecurityKey"];
                if (IsNullOrEmpty(securityKey)) 
                    return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Invalid Secret"));
                
                var refreshToken = GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = UtcNow.AddHours(24);

                await userManager.UpdateAsync(user);
                
                var jwtSecurityToken = await GenerateJwtToken(user, validIssuer, validAudience, securityKey);
                var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                return Ok(new LoginResponse(accessToken, refreshToken, jwtSecurityToken.ValidTo));
            }
            catch (Exception)
            {
                return StatusCode(Status500InternalServerError,
                    new LoginResponse(status: "Error", message: "Login went wrong"));
            }
        }
        
        

        private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user, string jwtValidIssuer, string jwtValidAudience, string jwtSecurityKey)
        {
            var authClaims = new List<Claim>
            {
                new(Name, user.Email ?? throw new InvalidOperationException()),
                new(NameIdentifier, user.Id),
                new(JwtRegisteredClaimNames.Jti, NewGuid().ToString()),
            };

            var userRoles = await userManager.GetRolesAsync(user);
            if (userRoles is { Count: > 0 })
            {
                authClaims.AddRange(userRoles.Select(userRole => new Claim(Role, userRole)));
            }

            var token = new JwtSecurityToken(
                issuer: jwtValidIssuer,
                audience: jwtValidAudience,
                expires: UtcNow.AddSeconds(60),
                claims: authClaims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(UTF8.GetBytes(jwtSecurityKey)),
                    HmacSha256)
            );
            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
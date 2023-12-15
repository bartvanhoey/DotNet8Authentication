using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.DateTime;
using static System.String;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Convert = System.Convert;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [Route("api/account")]
    [ApiController]
    public class LoginController(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
        : AuthControllerBase(userManager)
    {
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginInputModel? input)
        {
            try
            {
                if (input is null)
                    return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Login went wrong"));

                var user = await userManager.FindByEmailAsync(input.Email);
                if (user == null)
                    return StatusCode(Status500InternalServerError,
                        new LoginResponse("Error", "Invalid Password or Username"));

                if (IsNullOrWhiteSpace(user.Email))
                    return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Email Empty"));

                var isPasswordValid = await userManager.CheckPasswordAsync(user, input.Password);
                if (!isPasswordValid)
                    return StatusCode(Status500InternalServerError,
                        new LoginResponse("Error", "Invalid Password or Username"));

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

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
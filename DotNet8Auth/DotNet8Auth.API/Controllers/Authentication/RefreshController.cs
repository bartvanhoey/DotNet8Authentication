using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Login;
using DotNet8Auth.Shared.Models.Authentication.Refresh;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static System.String;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication;

[Route("api/account")]
[ApiController]
public class RefreshController(
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    ILogger<RefreshController> logger)
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    : AuthControllerBase(userManager, configuration)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [HttpPost("Refresh")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status500InternalServerError)]
    public async Task<IActionResult> Refresh([FromBody] RefreshInputModel model)
    {
        try
        {
            if (IsNullOrWhiteSpace(model.AccessToken))
            {
                logger.LogError($"{nameof(Refresh)}: access token is null");
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", "AccessToken Empty"));
            }

            if (IsNullOrWhiteSpace(model.RefreshToken))
            {
                logger.LogError($"{nameof(Refresh)}: refresh token is null");
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", "RefreshToken Empty"));
            }

            var validIssuer = configuration["Jwt:ValidIssuer"];
            if (IsNullOrEmpty(validIssuer))
            {
                logger.LogError($"{nameof(Refresh)}: valid issuer is null");
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Invalid Issuer"));
            }

            var validAudience = configuration["Jwt:ValidAudience"];
            if (IsNullOrEmpty(validAudience))
            {
                logger.LogError($"{nameof(Refresh)}: valid audience is null");
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Invalid Audience"));
            }

            var securityKey = configuration["Jwt:SecurityKey"];
            if (IsNullOrEmpty(securityKey))
            {
                logger.LogError($"{nameof(Refresh)}: security key is null");
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Invalid Secret"));
            }

            var principal = GetPrincipalFromExpiredToken(model.AccessToken, securityKey, validIssuer, validAudience);
            if (principal?.Identity?.Name is null)
            {
                logger.LogError($"{nameof(Refresh)}: principal is null");
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Principal null"));
            }

            var user = await userManager.FindByNameAsync(principal.Identity.Name);
            if (user is null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                logger.LogError($"{nameof(Refresh)}: something wrong with refresh token");
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Refresh went wrong"));
            }

            var jwtSecurityToken = await GenerateJwtToken(user, validIssuer, validAudience, securityKey);

            return Ok(new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ValidTo = jwtSecurityToken.ValidTo,
                RefreshToken = model.RefreshToken
            });
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(Refresh));
            return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Refresh went wrong"));
        }
    }

    private static ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, string securityKey,
        string validIssuer, string validAudience)
    {
        var validation = new TokenValidationParameters
        {
            ValidIssuer = validIssuer,
            ValidAudience = validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
            ValidateLifetime = false
        };
        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }
}
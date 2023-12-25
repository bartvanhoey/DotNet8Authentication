using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNet8Auth.API.Controllers.Authentication.Base;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Refresh;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication;

[Route("api/account")]
[ApiController]
public class RefreshController(
    UserManager<ApplicationUser> userManager,
    IHostEnvironment environment,
    IConfiguration configuration,
    ILogger<RefreshController> logger)
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    : AuthControllerBase(userManager, configuration, environment)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [HttpPost("Refresh")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status500InternalServerError)]
    public async Task<IActionResult> Refresh([FromBody] RefreshInputModel model)
    {
        try
        {
            var validationResult = ValidateControllerInputModel(model, logger, nameof(Refresh));
            if (validationResult.IsFailure) return Nok500<RefreshResponse>(logger, validationResult.Error?.Message);

            if (IsNullOrWhiteSpace(model.AccessToken)) return Nok500<RefreshResponse>(logger, "Access token empty");

            if (IsNullOrWhiteSpace(model.RefreshToken)) return Nok500<RefreshResponse>(logger, "Refresh token empty");

            var principal = GetPrincipalFromExpiredToken(model.AccessToken, validationResult.Value.SecurityKey,
                validationResult.Value.ValidIssuer, validationResult.Value.Origin);
            if (principal?.Identity?.Name is null) return Nok500<RefreshResponse>(logger, "Principal null");

            var user = await userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null) return Nok500CouldNotFindUser<RefreshResponse>(logger);

            if (user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
                return Nok500<RefreshResponse>(logger, "Something wrong with Refresh token");

            var jwtSecurityToken = await GenerateJwtToken(user, validationResult.Value.ValidIssuer,
                validationResult.Value.Origin, validationResult.Value.SecurityKey);
           
            return Ok(new RefreshResponse(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                model.RefreshToken, jwtSecurityToken.ValidTo));
        }
        catch (Exception exception)
        {
            return Nok500<RefreshResponse>(logger, exception);
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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Login;
using DotNet8Auth.Shared.Models.Authentication.Refresh;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [Route("api/Account")]
    [ApiController]
    public class RefreshController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        : AuthControllerBase(userManager)
    {
        // private readonly ILogger<RefreshController> _logger;

        // _logger = logger;


        [HttpPost("Refresh")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<IActionResult> Refresh([FromBody] RefreshInputModel model)
        {
            // _logger.LogInformation("Refresh called");
            
            if (string.IsNullOrWhiteSpace(model.AccessToken))
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", "AccessToken Empty"));
            
            if (string.IsNullOrWhiteSpace(model.RefreshToken))
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", "RefreshToken Empty"));
            
            var validIssuer = configuration["Jwt:ValidIssuer"];
            if (string.IsNullOrEmpty(validIssuer)) 
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Invalid Issuer"));

            var validAudience = configuration["Jwt:ValidAudience"];
            if (string.IsNullOrEmpty(validAudience)) 
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Invalid Audience"));

            var securityKey = configuration["Jwt:SecurityKey"];
            if (string.IsNullOrEmpty(securityKey)) 
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Invalid Secret"));
            
            var principal = GetPrincipalFromExpiredToken(model.AccessToken, securityKey, validIssuer, validAudience);
            if (principal?.Identity?.Name is null)
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Principal null"));

            var user = await userManager.FindByNameAsync(principal.Identity.Name);
            if (user is null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Refresh went wrong"));
            
            var jwtSecurityToken = await GenerateJwtToken(user, validIssuer, validAudience, securityKey);

            // _logger.LogInformation("Refresh succeeded");

            return Ok(new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ValidTo = jwtSecurityToken.ValidTo,
                RefreshToken = model.RefreshToken
            });
        }

        private static ClaimsPrincipal? GetPrincipalFromExpiredToken (string token, string securityKey, string validIssuer, string validAudience) 
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
}
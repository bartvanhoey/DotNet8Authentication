using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication;

[Route("api/account")]
[ApiController]
public class RevokeController(UserManager<ApplicationUser> userManager, IHostEnvironment environment, ILogger<RevokeController> logger, IConfiguration configuration)
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    : AuthControllerBase(userManager, configuration, environment)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [Authorize]
    [HttpDelete("Revoke")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status401Unauthorized)]
    public async Task<IActionResult> Revoke()
    {
        try
        {
            var origin = ValidateOrigin(logger, nameof(Revoke));
            if (origin.IsFailure) return Unauthorized();

            var username = HttpContext.User.Identity?.Name;
            if (username is null) return Unauthorized();

            var user = await userManager.FindByNameAsync(username);
            if (user is null) return Unauthorized();
 
            user.RefreshToken = null;
            await userManager.UpdateAsync(user);
            return Ok();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(Revoke));
            return Ok();
        }
    }
}
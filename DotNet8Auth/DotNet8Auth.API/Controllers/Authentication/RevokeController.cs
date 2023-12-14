using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [Route("api/Account")]
    [ApiController]
    public class RevokeController(UserManager<ApplicationUser> userManager)
        : ControllerBase
    {
        // private readonly ILogger<RefreshController> _logger;

        // _logger = logger;

        [Authorize]
        [HttpDelete("Revoke")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status401Unauthorized)]
        public async Task<IActionResult> Revoke()
        {
            // _logger.LogInformation("Revoke called");

            var username = HttpContext.User.Identity?.Name;
            if (username is null) return Unauthorized();

            var user = await userManager.FindByNameAsync(username);
            if (user is null) return Unauthorized();

            user.RefreshToken = null;
            await userManager.UpdateAsync(user);

            // _logger.LogInformation("Revoke succeeded");

            return Ok();
        }
    }
}
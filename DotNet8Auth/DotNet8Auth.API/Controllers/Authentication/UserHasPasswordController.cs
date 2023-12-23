using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ChangePassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication;

[Route("api/account")]
[ApiController]
public class UserHasPasswordController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<UserHasPasswordController> logger) : AuthControllerBase(userManager, configuration)
{
    [Authorize]
    [HttpGet]
    [Route("user-has-password")]
    public async Task<IActionResult> GetUserHasPassword()
    {
        try
        {
            var result = ValidateController(logger, nameof(GetUserHasPassword));
            if (result.IsFailure)
                return StatusCode(Status500InternalServerError, new UserHasPasswordResponse("Error", result.Error?.Message ?? "something went wrong"));

            var email = HttpContext.User.Identity?.Name;
            if (email.IsNullOrWhiteSpace())
            {
                logger.LogError($"{nameof(GetUserHasPassword)}: Email was null");
                return StatusCode(Status500InternalServerError,
                    new UserHasPasswordResponse("Error", "Email was null"));
            }

            var user = email == null ? null : await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                logger.LogError($"{nameof(GetUserHasPassword)}: User retrieval went wrong");
                return StatusCode(Status500InternalServerError,
                    new UserHasPasswordResponse("Error", "User retrieval went wrong"));
            }

            var hasPassword = await userManager.HasPasswordAsync(user);
            return Ok(new UserHasPasswordResponse("Success", hasPassword));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(GetUserHasPassword));
            return StatusCode(Status500InternalServerError, new UserHasPasswordResponse("Error", "An exception occurred"));
        }
    }


}
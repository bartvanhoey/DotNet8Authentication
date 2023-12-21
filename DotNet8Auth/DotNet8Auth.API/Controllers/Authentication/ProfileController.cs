using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication;

[Route("api/account")]
[ApiController]
public class ProfileController(
    UserManager<ApplicationUser> userManager,
    ILogger<ProfileController> logger,
    IConfiguration configuration)
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    : AuthControllerBase(userManager, configuration)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [Authorize]
    [HttpGet]
    [Route("get-profile")]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var model = new GetProfileInputModel();

            var result = ValidateInputModel(model, logger, nameof(GetProfile));
            if (result.IsFailure)
                return StatusCode(Status500InternalServerError,
                    new ProfileResponse("Error", result.Error?.Message ?? "something went wrong"));

            var email = HttpContext.User.Identity?.Name;
            if (email.IsNullOrWhiteSpace())
            {
                logger.LogError($"{nameof(GetProfile)}: Email was null");
                return StatusCode(Status500InternalServerError,
                    new ProfileResponse("Error", result.Error?.Message ?? "something went wrong"));
            }

            var user = email == null ? null : await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                logger.LogError($"{nameof(GetProfile)}: User retrieval went wrong ");
                return StatusCode(Status500InternalServerError,
                    new ProfileResponse("Error", "User retrieval went wrong"));
            }

            var userName = await userManager.GetUserNameAsync(user);
            var phoneNumber = await userManager.GetPhoneNumberAsync(user);
            return Ok(new ProfileResponse("Success", userName: userName, phoneNumber: phoneNumber));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(GetProfile));
            return StatusCode(Status500InternalServerError, new ProfileResponse("Error", "An exception occurred"));
        }
    }
}
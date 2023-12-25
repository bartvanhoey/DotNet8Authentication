using DotNet8Auth.API.Controllers.Authentication.Base;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNet8Auth.API.Controllers.Authentication;

[Route("api/account")]
[ApiController]
public class ProfileController(UserManager<ApplicationUser> userManager, IHostEnvironment environment,
    ILogger<ProfileController> logger,
    IConfiguration configuration)
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    : AuthControllerBase(userManager, configuration, environment)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [Authorize]
    [HttpGet]
    [Route("get-profile")]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var result = ValidateController(logger, nameof(GetProfile));
            if (result.IsFailure) return Nok500<ProfileResponse>(logger, result.Error?.Message);

            var email = HttpContext.User.Identity?.Name;
            if (email.IsNullOrWhiteSpace()) return Nok500EmailIsNull<ProfileResponse>(logger);

            var user = await userManager.FindByEmailAsync(email ?? throw new InvalidOperationException());
            if (user == null) return Nok500CouldNotFindUser<ProfileResponse>(logger);

            var userName = await userManager.GetUserNameAsync(user);
            var phoneNumber = await userManager.GetPhoneNumberAsync(user);
            return Ok(new ProfileResponse("Success", userName, phoneNumber));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(GetProfile));
            return Nok500<ProfileResponse>(logger);
        }
    }
}
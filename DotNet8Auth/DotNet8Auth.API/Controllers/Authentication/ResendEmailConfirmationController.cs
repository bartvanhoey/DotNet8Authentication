using System.Text;
using DotNet8Auth.API.Controllers.Authentication.Base;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ResendEmailConfirmation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication;

[ApiController]
[Route("api/account")]
public class ResendEmailConfirmationController(UserManager<ApplicationUser> userManager, IHostEnvironment environment,
    IEmailSender<ApplicationUser> emailSender,
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    IConfiguration configuration,
    ILogger<ResendEmailConfirmationController> logger) : AuthControllerBase(userManager, configuration, environment)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [HttpPost]
    [Route("resend-email-confirmation")]
    public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailConfirmationInputModel model)
    {
        try
        {
            var result = ValidateControllerInputModel(model, logger, nameof(ResendEmailConfirmation));
            if (result.IsFailure)
                return StatusCode(Status500InternalServerError,
                    new ResendEmailConfirmationResponse("Error", result.Error?.Message ?? "something went wrong"));

            var callbackUrl = $"{HttpContext.Request.Headers.Origin}/Account/ConfirmEmail";

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                logger.LogError($"{nameof(ResendEmailConfirmation)}: user is null");
                return StatusCode(Status500InternalServerError,
                    new ResendEmailConfirmationResponse("Error", "User could not be found"));
            }

            var userId = await userManager.GetUserIdAsync(user);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var confirmationLink = callbackUrl.AddUrlParameters(new Dictionary<string, object?>
                { ["userId"] = userId, ["code"] = code, ["returnUrl"] = null });

            await emailSender.SendConfirmationLinkAsync(user, model.Email, confirmationLink);

            return Ok(new ResendEmailConfirmationResponse("Success", "Resend Email Confirmation successful", code,
                userId));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(ResendEmailConfirmation));
            return StatusCode(Status500InternalServerError,
                new ResendEmailConfirmationResponse("Error", "Something went wrong"));
        }
    }
}
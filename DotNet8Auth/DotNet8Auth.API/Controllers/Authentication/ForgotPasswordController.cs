using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ForgotPassword;
using DotNet8Auth.Shared.Models.Authentication.ResendEmailConfirmation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Text.Encoding;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Microsoft.AspNetCore.WebUtilities.WebEncoders;

namespace DotNet8Auth.API.Controllers.Authentication;

[ApiController]
[Route("api/account")]
public class ForgotPasswordController(
    UserManager<ApplicationUser> userManager,
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    IEmailSender<ApplicationUser> emailSender, IConfiguration configuration,ILogger<ForgotPasswordController> logger) : AuthControllerBase(userManager, configuration)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [HttpPost]
    [Route("forgot-password")]
    public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailConfirmationInputModel model)
    {
        try
        {
            var validationResult = ValidateInputModel(model, logger, nameof(ResendEmailConfirmation));
            if (validationResult.IsFailure) 
                return StatusCode(Status500InternalServerError, new ForgotPasswordResponse("Error", validationResult.Error?.Message ?? "something went wrong"));
            
            var callbackUrl = $"{HttpContext.Request.Headers.Origin}/Account/ResetPassword";
            
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                logger.LogError($"{nameof(ResendEmailConfirmation)}: user is null");
                return StatusCode(Status500InternalServerError,
                    new ForgotPasswordResponse("Error", "Forgot password retrieving user went wrong"));
            }

            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                logger.LogError($"{nameof(ResendEmailConfirmation)}: User email is not confirmed");
                return StatusCode(Status500InternalServerError,
                    new ForgotPasswordResponse("Error", "Forgot password went wrong"));
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = Base64UrlEncode(UTF8.GetBytes(code));
            var resetLink = callbackUrl.AddUrlParameters(new Dictionary<string, object?> { ["code"] = code });

            await emailSender.SendPasswordResetLinkAsync(user, model.Email, resetLink);

            return Ok(new ForgotPasswordResponse("Success", "Resend Email Confirmation successful", code));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(ResendEmailConfirmation));
            return StatusCode(Status500InternalServerError,
                new ForgotPasswordResponse("Error", "Forgot password went wrong"));
        }
    }
}
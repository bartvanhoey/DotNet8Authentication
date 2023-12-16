using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ForgotPassword;
using DotNet8Auth.Shared.Models.Authentication.ResendEmailConfirmation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Text.Encoding;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Microsoft.AspNetCore.WebUtilities.WebEncoders;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/account")]
    public class ForgotPasswordController(
        UserManager<ApplicationUser> userManager,
        IEmailSender<ApplicationUser> emailSender, IConfiguration configuration,ILogger<ForgotPasswordController> logger) : ControllerBase
    {
        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailConfirmationInputModel input)
        {
            try
            {
                var validAudience = configuration["Jwt:ValidAudience"];
                if (string.IsNullOrEmpty(validAudience))
                {
                    logger.LogError($"{nameof(ResendEmailConfirmation)}: valid audience is null");
                    return StatusCode(Status500InternalServerError, new ForgotPasswordResponse("Error", "Invalid Audience"));
                }

                var origin = HttpContext.Request.Headers.Origin;
                if (validAudience != origin)
                {
                    logger.LogError($"{nameof(ResendEmailConfirmation)}: origin is invalid");
                    return StatusCode(Status500InternalServerError, new ForgotPasswordResponse("Error", "Invalid Origin"));
                }

                var callbackUrl = $"{origin}/Account/ResetPassword";
            
                var user = await userManager.FindByEmailAsync(input.Email);
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

                await emailSender.SendPasswordResetLinkAsync(user, input.Email, resetLink);

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
}
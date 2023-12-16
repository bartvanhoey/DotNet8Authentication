using System.Text;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ResendEmailConfirmation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using static System.String;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/account")]
    public class ResendEmailConfirmationController(
        UserManager<ApplicationUser> userManager,
        IEmailSender<ApplicationUser> emailSender,
        IConfiguration configuration, ILogger<ResendEmailConfirmationController> logger) : ControllerBase
    {
        [HttpPost]
        [Route("resend-email-confirmation")]
        public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailConfirmationInputModel model)
        {
            try
            {
                var validAudience = configuration["Jwt:ValidAudience"];
                if (IsNullOrEmpty(validAudience))
                {
                    logger.LogError($"{nameof(ResendEmailConfirmation)}: audience is null");
                    return StatusCode(Status500InternalServerError,
                        new ResendEmailConfirmationResponse("Error", "Invalid Audience"));
                }

                var origin = HttpContext.Request.Headers.Origin;
                if (validAudience != origin)
                {
                    logger.LogError($"{nameof(ResendEmailConfirmation)}: origin is wrong");
                    return StatusCode(Status500InternalServerError,
                        new ResendEmailConfirmationResponse("Error", "Invalid Audience"));
                }

                var callbackUrl = $"{origin}/Account/ConfirmEmail";

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
}
using System.Text;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using static Microsoft.AspNetCore.Http.StatusCodes;
using System.Text.Encodings.Web;
using DotNet8Auth.Shared.Models.Authentication.ChangeEmail;
using Microsoft.AspNetCore.Authorization;

namespace DotNet8Auth.API.Controllers.Authentication;

[ApiController]
[Route("api/account")]
public class ChangeEmailController(UserManager<ApplicationUser> userManager, IEmailSender<ApplicationUser> emailSender,
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    IConfiguration configuration, ILogger<ChangeEmailController> logger) : AuthControllerBase(userManager, configuration)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{

    [HttpPost]
    [Authorize]
    [Route("change-email")]
    public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailInputModel model)
    {
        try
        {
            var result = ValidateControllerInputModel(model, logger, nameof(ChangeEmail));
            if (result.IsFailure)
                return StatusCode(Status500InternalServerError,
                    new ChangeEmailResponse("Error", result.Error?.Message ?? "something went wrong"));

            var email = HttpContext.User.Identity?.Name;
            if (email.IsNullOrWhiteSpace())
            {
                logger.LogError($"{nameof(ChangeEmail)}: Email was null");
                return StatusCode(Status500InternalServerError, new ChangeEmailResponse("Error", "Email was null"));
            }

            var user = email == null ? null : await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                logger.LogError($"{nameof(ChangeEmail)}: User retrieval went wrong");
                return StatusCode(Status500InternalServerError, new ChangeEmailResponse("Error", "User retrieval went wrong"));
            }
            
            var code = await userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail ?? throw new InvalidOperationException("NewEmail was null"));
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = $"{HttpContext.Request.Headers.Origin}/Account/ConfirmEmailChange";
            var userId = await userManager.GetUserIdAsync(user);
            var confirmationLink = callbackUrl.AddUrlParameters(new Dictionary<string, object?>
                { ["userId"] = userId, ["email"] = model.NewEmail, ["code"] = code });

            await emailSender.SendConfirmationLinkAsync(user, model.NewEmail, HtmlEncoder.Default.Encode(confirmationLink));

            return Ok(new ChangeEmailResponse("Success", "Resend Email Confirmation successful"));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(ChangeEmail));
            return StatusCode(Status500InternalServerError, new ChangeEmailResponse("Error", "Something went wrong"));
        }

    }
}
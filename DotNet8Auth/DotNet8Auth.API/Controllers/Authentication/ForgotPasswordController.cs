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
public class ForgotPasswordController(UserManager<ApplicationUser> userManager, IHostEnvironment environment,
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    IEmailSender<ApplicationUser> emailSender, IConfiguration configuration,ILogger<ForgotPasswordController> logger) : AuthControllerBase(userManager, configuration, environment)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [HttpPost]
    [Route("forgot-password")]
    public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailConfirmationInputModel model)
    {
        try
        {
            var validationResult = ValidateControllerInputModel(model, logger, nameof(ResendEmailConfirmation));
            if (validationResult.IsFailure) return Nok500<ForgotPasswordResponse>(logger, validationResult.Error?.Message);
            
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null) return Nok500CouldNotFindUser<ForgotPasswordResponse>(logger);

            if (!await userManager.IsEmailConfirmedAsync(user)) return Nok500<ForgotPasswordResponse>(logger, "Email not confirmed");

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = Base64UrlEncode(UTF8.GetBytes(code));
            var callbackUrl = $"{HttpContext.Request.Headers.Origin}/Account/ResetPassword";
            var resetLink = callbackUrl.AddUrlParameters(new Dictionary<string, object?> { ["code"] = code });

            await emailSender.SendPasswordResetLinkAsync(user, model.Email, resetLink);

            return Ok(new ForgotPasswordResponse("Success", "Resend Email Confirmation successful", code));
        }
        catch (Exception exception)
        {
            return Nok500<ForgotPasswordResponse>(logger, exception); 
        }
    }
}
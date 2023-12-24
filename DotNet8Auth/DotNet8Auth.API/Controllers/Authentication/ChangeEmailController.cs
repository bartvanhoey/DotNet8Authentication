using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using DotNet8Auth.Shared.Models.Authentication.ChangeEmail;
using Microsoft.AspNetCore.Authorization;
using static System.Text.Encoding;
using static Microsoft.AspNetCore.WebUtilities.WebEncoders;

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
            if (result.IsFailure) return Nok500<ChangeEmailResponse>(logger);
                
            var email = HttpContext.User.Identity?.Name;
            if (email.IsNullOrWhiteSpace()) return Nok500EmailIsNull<ChangeEmailResponse>(logger );

            var user = email == null ? null : await userManager.FindByEmailAsync(email);
            if (user == null) return Nok500CouldNotFindUser<ChangeEmailResponse>(logger);
            
            var code = await userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail ?? throw new InvalidOperationException("NewEmail was null"));
            code = Base64UrlEncode(UTF8.GetBytes(code));

            var callbackUrl = $"{HttpContext.Request.Headers.Origin}/Account/ConfirmEmailChange";
            var userId = await userManager.GetUserIdAsync(user);
            var confirmationLink = callbackUrl.AddUrlParameters(new Dictionary<string, object?>
                { ["userId"] = userId, ["email"] = user.Email, ["newEmail"] = model.NewEmail, ["code"] = code });

            await emailSender.SendConfirmationLinkAsync(user, model.NewEmail, HtmlEncoder.Default.Encode(confirmationLink));

            return Ok200<ChangeEmailResponse>("Resend Email Confirmation successful");
        }
        catch (Exception exception)
        {
            return Nok500<ChangeEmailResponse>(logger, exception);
        }

    }
}
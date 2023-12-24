using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ChangeEmail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Text.Encoding;
using static Microsoft.AspNetCore.WebUtilities.WebEncoders;

namespace DotNet8Auth.API.Controllers.Authentication;

[ApiController]
[Route("api/account")]
public class ConfirmChangeEmailController(
    UserManager<ApplicationUser> userManager,
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    IConfiguration configuration,
    ILogger<ChangeEmailController> logger) : AuthControllerBase(userManager, configuration)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [HttpPost]
    [Route("confirm-change-email")]
    public async Task<IActionResult> ConfirmChangeEmail([FromBody] ConfirmChangeEmailInputModel model)
    {
        try
        {
            var validationResult = ValidateControllerInputModel(model, logger, nameof(ConfirmChangeEmail));
            if (validationResult.IsFailure) return Nok500<ConfirmChangeEmailResponse>(logger, validationResult.Error?.Message);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null) return Nok500CouldNotFindUser<ConfirmChangeEmailResponse>(logger);

            if (model.Code.IsNullOrWhiteSpace()) return Nok500CodeIsNull<ConfirmChangeEmailResponse>(logger);

            var code = UTF8.GetString(Base64UrlDecode(model.Code));

            var changeEmailResult = await userManager.ChangeEmailAsync(user, model.NewEmail, code);
            if (!changeEmailResult.Succeeded) return Nok500<ConfirmChangeEmailResponse>(changeEmailResult.Errors, logger);
           
            var setUserNameResult = await userManager.SetUserNameAsync(user, model.NewEmail); // change user name also
            if (setUserNameResult is { Succeeded: true }) return Ok200<ConfirmChangeEmailResponse>("Email confirmed successfully");

            await userManager.ChangeEmailAsync(user, model.Email ?? throw new InvalidOperationException(),
                code); // if user name could not be changed, set email back to old email

            return Nok500<ConfirmChangeEmailResponse>(setUserNameResult.Errors, logger);
        }
        catch (Exception exception)
        {
            return Nok500<ConfirmChangeEmailResponse>(logger, exception);
        }
    }
}
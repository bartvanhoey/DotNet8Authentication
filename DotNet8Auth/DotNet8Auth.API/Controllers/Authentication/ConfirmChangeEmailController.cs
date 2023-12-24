using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ChangeEmail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Text.Encoding;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Microsoft.AspNetCore.WebUtilities.WebEncoders;

namespace DotNet8Auth.API.Controllers.Authentication;

[ApiController]
[Route("api/account")]
public class ConfirmChangeEmailController(UserManager<ApplicationUser> userManager,
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    IConfiguration configuration, ILogger<ChangeEmailController> logger) : AuthControllerBase(userManager, configuration)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{

    [HttpPost]
    [Authorize]
    [Route("confirm-change-email")]
    public async Task<IActionResult> ConfirmChangeEmail([FromBody] ConfirmChangeEmailInputModel model)
    {
        try
        {
            var validationResult = ValidateControllerInputModel(model, logger, nameof(ConfirmChangeEmail));
            if (validationResult.IsFailure)
                return StatusCode(Status500InternalServerError,
                    new ConfirmChangeEmailResponse("Error", validationResult.Error?.Message ?? "something went wrong"));

            var email = HttpContext.User.Identity?.Name;
            if (email.IsNullOrWhiteSpace())
            {
                logger.LogError($"{nameof(ConfirmChangeEmail)}: Email was null");
                return StatusCode(Status500InternalServerError, new ConfirmChangeEmailResponse("Error", "Email was null"));
            }

            var user = email == null ? null : await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                logger.LogError($"{nameof(ConfirmChangeEmail)}: User retrieval went wrong");
                return StatusCode(Status500InternalServerError, new ConfirmChangeEmailResponse("Error", "User retrieval went wrong"));
            }

            if (model.Code.IsNullOrWhiteSpace())
            {
                logger.LogError($"{nameof(ConfirmChangeEmail)}: Code is null");
                return StatusCode(Status500InternalServerError, new ConfirmChangeEmailResponse("Error", "Code is null"));
            }  
            var code = UTF8.GetString(Base64UrlDecode(model.Code));
                
            var changeEmailResult = await userManager.ChangeEmailAsync(user, model.NewEmail, code);
            if (changeEmailResult.Succeeded)
            {
                var setUserNameResult = await userManager.SetUserNameAsync(user, model.NewEmail);
                if (setUserNameResult is { Succeeded: true }) // change user name also
                    return Ok(new ConfirmChangeEmailResponse("Success", "Email confirmed successfully"));
                
                await userManager.ChangeEmailAsync(user, email ?? throw new InvalidOperationException(), code); // if user name could not be changed, set email back to old email
                
                var updateUserError = $"{setUserNameResult.Errors.FirstOrDefault()?.Code}: {setUserNameResult.Errors.FirstOrDefault()?.Description} ";
                logger.LogError("{ConfirmChangeEmailName}: {UpdateUserError}", nameof(ConfirmChangeEmail), updateUserError);
                return StatusCode(Status500InternalServerError, new ConfirmChangeEmailResponse("Error", setUserNameResult.Errors.Select(x => new ConfirmChangeEmailError(x.Code, x.Description))));
            }

            var confirmChangeEmailError =
                $"{changeEmailResult.Errors.FirstOrDefault()?.Code}: {changeEmailResult.Errors.FirstOrDefault()?.Description} ";
            logger.LogError("{ConfirmChangeEmailName}: {ConfirmChangeEmailError}", nameof(ConfirmChangeEmail),
                confirmChangeEmailError);
            return StatusCode(Status500InternalServerError,
                new ConfirmChangeEmailResponse("Error",
                    changeEmailResult.Errors.Select(x => new ConfirmChangeEmailError(x.Code, x.Description))));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(ConfirmChangeEmail));
            return StatusCode(Status500InternalServerError, new ConfirmChangeEmailResponse("Error", "Something went wrong"));
        }
    }
}
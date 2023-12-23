using System.Text;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ChangeEmail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/account")]
    public class ConfirmChangeEmailController(UserManager<ApplicationUser> userManager, IEmailSender<ApplicationUser> emailSender,
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
                    return StatusCode(StatusCodes.Status500InternalServerError,
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

                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
                if (code.IsNullOrWhiteSpace())
                {
                    logger.LogError($"{nameof(ConfirmChangeEmail)}: Code is null");
                    return StatusCode(Status500InternalServerError, new ConfirmChangeEmailResponse("Error", "Code is null"));
                }                
                
                IdentityResult? updateUserResult =null;
                var changeEmailResult = await userManager.ChangeEmailAsync(user, model.NewEmail, code);
                if (changeEmailResult.Succeeded){
                    user.UserName = model.NewEmail;
                    updateUserResult = await userManager.UpdateAsync(user);
                }  

                if (updateUserResult != null && updateUserResult.Succeeded && changeEmailResult.Succeeded)
                    return Ok(new ConfirmChangeEmailResponse("Success", "Email confirmed successfully"));    
                                
                if (updateUserResult != null && !updateUserResult.Succeeded)
                {
                    var updateUserError = $"{updateUserResult.Errors.FirstOrDefault()?.Code}: {updateUserResult.Errors.FirstOrDefault()?.Description} ";
                    logger.LogError($"{nameof(ConfirmChangeEmail)}: {updateUserError}");
                    return StatusCode(Status500InternalServerError, new ConfirmChangeEmailResponse("Error", updateUserResult.Errors.Select(x => new ConfirmChangeEmailError(x.Code, x.Description))));
                }

                var confirmChangeEmailError = $"{changeEmailResult.Errors.FirstOrDefault()?.Code}: {changeEmailResult.Errors.FirstOrDefault()?.Description} ";
                logger.LogError($"{nameof(ConfirmChangeEmail)}: {confirmChangeEmailError}");
                return StatusCode(Status500InternalServerError, new ConfirmChangeEmailResponse("Error", changeEmailResult.Errors.Select(x => new ConfirmChangeEmailError(x.Code, x.Description))));
            }
            catch (Exception exception)
            {
                logger.LogError(exception, nameof(ConfirmChangeEmail));
                return StatusCode(Status500InternalServerError, new ConfirmChangeEmailResponse("Error", "Something went wrong"));
            }
        }
    }
}
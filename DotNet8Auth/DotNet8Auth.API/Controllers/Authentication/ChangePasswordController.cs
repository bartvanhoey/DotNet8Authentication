using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ChangePassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [Route("api/account")]
    public class ChangePasswordController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<ChangePasswordController> logger) : AuthControllerBase(userManager, configuration)
    {
        [HttpPost]
        [Authorize]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInputModel model)
        {
            try
            {
                var validationResult = ValidateInputModel(model, logger, nameof(ChangePassword));
                if (validationResult.IsFailure)
                    return StatusCode(Status500InternalServerError,
                        new ChangePasswordResponse("Error", validationResult.Error?.Message ?? "something went wrong"));

                if (model.NewPassword.IsNullOrWhiteSpace() || model.OldPassword.IsNullOrWhiteSpace())
                    return StatusCode(Status500InternalServerError,
                            new ChangePasswordResponse("Error", "Old or New password is null or empty"));

                var email = HttpContext.User.Identity?.Name;
                if (email.IsNullOrWhiteSpace())
                {
                    logger.LogError($"{nameof(ChangePassword)}: Email was null");
                    return StatusCode(Status500InternalServerError,
                        new ChangePasswordResponse("Error", "email was null"));
                }

                var user = email == null ? null : await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    logger.LogError($"{nameof(ChangePassword)}: User retrieval went wrong");
                    return StatusCode(Status500InternalServerError,
                        new ChangePasswordResponse("Error", "User retrieval went wrong"));
                }

                var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded) return Ok(new ChangePasswordResponse("Success"));

                var changePasswordError =  $"{result.Errors.FirstOrDefault()?.Code}: {result.Errors.FirstOrDefault()?.Description} ";

                logger.LogError($"{nameof(ChangePassword)}: {changePasswordError}");
                return StatusCode(Status500InternalServerError, new ChangePasswordResponse("Error", result.Errors.Select(x => new ChangePasswordError(x.Code, x.Description))));
            }
            catch (Exception exception)
            {
                logger.LogError(exception, nameof(ChangePassword));
                return StatusCode(Status500InternalServerError,
                    new ChangePasswordResponse("Error", "Change password went wrong"));
            }
        }
    }
}
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ChangePassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNet8Auth.API.Controllers.Authentication;

[Route("api/account")]
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
public class ChangePasswordController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<ChangePasswordController> logger) : AuthControllerBase(userManager, configuration)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [HttpPost]
    [Authorize]
    [Route("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInputModel model)
    {
        try
        {
            var validationResult = ValidateControllerInputModel(model, logger, nameof(ChangePassword));
            if (validationResult.IsFailure) return Nok500<ChangePasswordResponse>(logger, validationResult.Error?.Message);

            if (model.NewPassword.IsNullOrWhiteSpace() || model.OldPassword.IsNullOrWhiteSpace())
                return Nok500<ChangePasswordResponse>(logger, "Old or New password is null or empty");

            var email = HttpContext.User.Identity?.Name;
            if (email.IsNullOrWhiteSpace()) return Nok500EmailIsNull<ChangePasswordResponse>(logger);

            var user = email == null ? null : await userManager.FindByEmailAsync(email);
            if (user == null) return Nok500CouldNotFindUser<ChangePasswordResponse>(logger);

            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            return changePasswordResult.Succeeded 
                ? Ok200<ChangePasswordResponse>() 
                : Nok500<ChangePasswordResponse>(changePasswordResult.Errors, logger);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(ChangePassword));
            return Nok500<ChangePasswordResponse>(logger, exception, "Change password went wrong"); 
        }
    }
}
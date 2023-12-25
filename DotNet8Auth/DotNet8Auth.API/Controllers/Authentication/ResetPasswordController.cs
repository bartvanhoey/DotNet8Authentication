using System.Drawing;
using System.Text;
using DotNet8Auth.API.Controllers.Authentication.Base;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Login;
using DotNet8Auth.Shared.Models.Authentication.ResetPassword;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication;

[ApiController]
[Route("api/account")]
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
public class ResetPasswordController(UserManager<ApplicationUser> userManager, IHostEnvironment environment, ILogger<ResetPasswordController> logger, IConfiguration configuration) : AuthControllerBase(userManager, configuration, environment)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [HttpPost]
    [Route("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordInputModel model)
    {
        try
        {
            var validationResult = ValidateControllerInputModel(model, logger, nameof(ResetPassword));
            if (validationResult.IsFailure) 
                return StatusCode(Status500InternalServerError, new ResetPasswordResponse("Error", validationResult.Error?.Message ?? "something went wrong"));

            
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                logger.LogError($"{nameof(ResetPassword)}: user is null");
                return StatusCode(Status500InternalServerError,
                    new ResetPasswordResponse(status: "Error", message: "Reset password went wrong"));
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            var result = await userManager.ResetPasswordAsync(user, token, model.Password);

            if (result.Succeeded)
                return Ok(new ResetPasswordResponse { Status = "Success", Message = "Password reset successful" });
                
            logger.LogError($"{nameof(ResetPassword)}: reset password went wrong");
            return StatusCode(Status500InternalServerError,
                new ResetPasswordResponse { Status = "Error", Message = "Reset password went wrong" });
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(ResetPassword));
            return StatusCode(Status500InternalServerError,
                new ResetPasswordResponse { Status = "Error", Message = "Reset password went wrong" });
        }
    }
}
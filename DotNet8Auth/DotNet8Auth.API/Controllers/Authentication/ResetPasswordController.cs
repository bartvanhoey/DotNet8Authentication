using System.Text;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ResetPassword;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace DotNet8Auth.API.Controllers.Authentication;

[ApiController]
[Route("api/account")]
public class ResetPasswordController(UserManager<ApplicationUser> userManager, ILogger<ResetPasswordController> logger) : ControllerBase
{
    [HttpPost]
    [Route("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordInputModel model)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                logger.LogError($"{nameof(ResetPassword)}: user is null");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResetPasswordResponse { Status = "Error", Message = "Reset password went wrong" });
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            var result = await userManager.ResetPasswordAsync(user, token, model.Password);

            if (result.Succeeded)
                return Ok(new ResetPasswordResponse { Status = "Success", Message = "Password reset successful" });
                
            logger.LogError($"{nameof(ResetPassword)}: reset password went wrong");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ResetPasswordResponse { Status = "Error", Message = "Reset password went wrong" });
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(ResetPassword));
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ResetPasswordResponse { Status = "Error", Message = "Reset password went wrong" });
        }
    }
}
using System.Text;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ResetPassword;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/account")]
    public class ResetPasswordController(UserManager<ApplicationUser> userManager) : ControllerBase
    {
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordInputModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResetPasswordResponse { Status = "Error", Message = "Reset password went wrong" });

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            var result = await userManager.ResetPasswordAsync(user, token, model.Password);

            if (result.Succeeded)
                return Ok(new ResetPasswordResponse { Status = "Success", Message = "Password reset successful" });
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ResetPasswordResponse { Status = "Error", Message = "Reset password went wrong" });
        }
    }
}
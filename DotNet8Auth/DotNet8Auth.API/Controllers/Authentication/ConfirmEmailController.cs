using System.Text;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ConfirmEmail;
using DotNet8Auth.Shared.Models.Authentication.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/account")]
    public class ConfirmEmailController(UserManager<ApplicationUser> userManager) : ControllerBase
    {
        [HttpPost]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailInputModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ConfirmEmailResponse { Status = "Error", Message = "User does not exist." });

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            var result = await userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
                return Ok(new ConfirmEmailResponse { Status = "Success", Message = "Confirm email successful." });

            return StatusCode(StatusCodes.Status500InternalServerError,
                new RegisterResponse { Status = "Error", Message = "Confirm email failed! Please try again." });
        }
    }
}
using System.Text;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ResendEmailConfirmation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/Account")]
    public class ResendEmailConfirmationController(UserManager<ApplicationUser> userManager, IEmailSender<ApplicationUser> emailSender) : ControllerBase
    {
        [HttpPost]
        [Route("resend-email-confirmation")]
        public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailConfirmationInputModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResendEmailConfirmationResponse { Status = "Error", Message = "User could not be found" });

            var userId = await userManager.GetUserIdAsync(user);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var confirmationLink = model.CallbackUrl.AddUrlParameters(new Dictionary<string, object?>
                { ["userId"] = userId, ["code"] = code, ["returnUrl"] = null });

            await emailSender.SendConfirmationLinkAsync(user, model.Email, confirmationLink);

            return Ok(new ResendEmailConfirmationResponse
                { Status = "Success", Message = "Resend Email Confirmation successful", Code = code, UserId = userId });
        }
    }
}
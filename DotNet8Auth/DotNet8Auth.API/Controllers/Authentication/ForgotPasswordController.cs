using System.Text;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ForgotPassword;
using DotNet8Auth.Shared.Models.Authentication.ResendEmailConfirmation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/account")]
    public class ForgotPasswordController(
        UserManager<ApplicationUser> userManager,
        IEmailSender<ApplicationUser> emailSender, IConfiguration configuration) : ControllerBase
    {
        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailConfirmationInputModel input)
        {
            var validAudience = configuration["Jwt:ValidAudience"];
            if (string.IsNullOrEmpty(validAudience))
                return StatusCode(Status500InternalServerError, new ForgotPasswordResponse("Error", "Invalid Audience"));

            var origin = HttpContext.Request.Headers.Origin;
            if (validAudience != origin)
                return StatusCode(Status500InternalServerError, new ForgotPasswordResponse("Error", "Invalid Audience"));
                
            var callbackUrl = $"{origin}/Account/ResetPassword";
            
            var user = await userManager.FindByEmailAsync(input.Email);
            if (user == null)
                return StatusCode(Status500InternalServerError,
                    new ForgotPasswordResponse("Error", "Forgot password retrieving user went wrong"));

            if (!await userManager.IsEmailConfirmedAsync(user))
                return StatusCode(Status500InternalServerError,
                    new ForgotPasswordResponse("Error", "Forgot password went wrong"));
            
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var resetLink = callbackUrl.AddUrlParameters(new Dictionary<string, object?> { ["code"] = code });

            await emailSender.SendPasswordResetLinkAsync(user, input.Email, resetLink);

            return Ok(new ForgotPasswordResponse("Success", "Resend Email Confirmation successful", code));
        }
    }
}
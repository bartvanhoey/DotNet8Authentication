using System.Text;

using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/Account")]
    public class RegisterController(UserManager<ApplicationUser> userManager, IEmailSender<ApplicationUser> emailSender)
        : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputModel model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new RegisterResponse { Status = "Error", Message = "User already exists!" });

            var user = CreateUser();
            if (user == null) return StatusCode(StatusCodes.Status500InternalServerError, new RegisterResponse { Status = "Error", Message = "CreateUser failed" }); ;
            user.Email = model.Email;
            user.UserName = model.Email;

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var userId = await userManager.GetUserIdAsync(user);
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

               var confirmationLink = model.CallbackUrl.AddUrlParameters( new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code, ["returnUrl"] = null }) ?? "";


                await emailSender.SendConfirmationLinkAsync(user, user.Email, confirmationLink);

                return Ok(new RegisterResponse { Status = "Success", Message = "User created successfully!", Code = code, UserId = userId });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new RegisterResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." });
        }

        private static ApplicationUser? CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                return null;
            }
        }
    }
}
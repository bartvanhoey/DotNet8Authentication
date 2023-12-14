using System.Text;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using static System.Activator;
using static Microsoft.AspNetCore.Http.StatusCodes;

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
            if (string.IsNullOrEmpty(model.CallbackUrl)) return StatusCode(Status500InternalServerError, new RegisterResponse("Error", "Empty CallbackUrl"));
            
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
                return StatusCode(Status500InternalServerError, new RegisterResponse("Error", "User already exists!"));

            var newUser = CreateUser();
            if (newUser == null)
                return StatusCode(Status500InternalServerError, new RegisterResponse("Error", "CreateUser failed"));
            
            newUser.Email = model.Email;
            newUser.UserName = model.Email;

            var result = await userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
                return StatusCode(Status500InternalServerError, new RegisterResponse("Error", "User creation failed"));
            
            var userId = await userManager.GetUserIdAsync(newUser);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            

            var confirmationLink = model.CallbackUrl.AddUrlParameters(new Dictionary<string, object?>
                { ["userId"] = userId, ["code"] = code, ["returnUrl"] = null }) ?? "";

            await emailSender.SendConfirmationLinkAsync(newUser, newUser.Email, confirmationLink);
            return Ok(new RegisterResponse("Success", "User created successfully!", code, userId));
        }

        private static ApplicationUser? CreateUser()
        {
            try
            {
                return CreateInstance<ApplicationUser>();
            }
            catch
            {
                return null;
            }
        }
    }
}
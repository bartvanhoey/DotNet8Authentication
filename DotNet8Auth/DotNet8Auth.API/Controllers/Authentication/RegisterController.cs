using System.Text;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication.Login;
using DotNet8Auth.Shared.Models.Authentication.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using static System.Activator;

using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication;

[ApiController]
[Route("api/account")]
public class RegisterController(UserManager<ApplicationUser> userManager, IEmailSender<ApplicationUser> emailSender, IConfiguration configuration, ILogger<RegisterController> logger)
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    : AuthControllerBase(userManager, configuration)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterInputModel model)
    {
        try
        {
            var validationResult = ValidateInputModel(model, logger, nameof(Register));
            if (validationResult.IsFailure) 
                return StatusCode(Status500InternalServerError, new LoginResponse("Error", validationResult.Error?.Message ?? "something went wrong"));


            var callbackUrl = $"{HttpContext.Request.Headers.Origin.FirstOrDefault()}/Account/ConfirmEmail";
            if (IsNullOrEmpty(model.Email))
            {
                logger.LogError($"{nameof(Register)}: email is null");
                return StatusCode(Status500InternalServerError, new RegisterResponse("Error", "Empty Email"));
            }

            if (IsNullOrEmpty(model.Password))
            {
                logger.LogError($"{nameof(Register)}: password is null");
                return StatusCode(Status500InternalServerError, new RegisterResponse("Error", "Empty Password"));
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                logger.LogError($"{nameof(Register)}: user '{model.Email}' already exists");
                return StatusCode(Status500InternalServerError, new RegisterResponse("Error", "User already exists"));
            }

            var newUser = CreateUser();
            if (newUser == null)
            {
                logger.LogError($"{nameof(Register)}: new user is null");
                return StatusCode(Status500InternalServerError, new RegisterResponse("Error", "CreateUser failed"));
            }

            newUser.Email = model.Email;
            newUser.UserName = model.Email;

            var result = await userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(Status500InternalServerError, new RegisterResponse("Error", "User creation failed"));
            }

            var userId = await userManager.GetUserIdAsync(newUser);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            
            var confirmationLink = callbackUrl.AddUrlParameters(new Dictionary<string, object?>
                { ["userId"] = userId, ["code"] = code, ["returnUrl"] = null });

            await emailSender.SendConfirmationLinkAsync(newUser, newUser.Email, confirmationLink);
            return Ok(new RegisterResponse("Success", "User created successfully", code, userId));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(Register));
            return StatusCode(Status500InternalServerError, new RegisterResponse("Error", "Something went wrong"));
        }
    }
        
    // [HttpPost]
    // [Route("register-admin")]
    // public async Task<IActionResult> RegisterAdmin([FromBody] RegisterInputModel model)
    // {
    //     var userExists = await userManager.FindByNameAsync(model.Email);
    //     if (userExists != null)
    //         return StatusCode(Status500InternalServerError,
    //             new LoginResponse { Status = "Error", Message = "User already exists" });
    //
    //     ApplicationUser user = new ApplicationUser()
    //     {
    //         Email = model.Email,
    //         SecurityStamp = NewGuid().ToString(),
    //         UserName = model.Email
    //     };
    //     var result = await userManager.CreateAsync(user, model.Password);
    //     if (!result.Succeeded)
    //         return StatusCode(Status500InternalServerError,
    //             new LoginResponse
    //             {
    //                 Status = "Error", Message = "User creation failed! Please check user details and try again."
    //             });
    //
    //     if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
    //         await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
    //     if (!await roleManager.RoleExistsAsync(UserRoles.User))
    //         await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
    //
    //     if (await roleManager.RoleExistsAsync(UserRoles.Admin))
    //     {
    //         await userManager.AddToRoleAsync(user, UserRoles.Admin);
    //     }
    //
    //     return Ok(new LoginResponse { Status = "Success", Message = "User created successfully" });
    // }


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
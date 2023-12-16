using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [Route("api/account")]
    [ApiController]
    public class ProfileController(
        UserManager<ApplicationUser> userManager, ILogger<ProfileController> logger)
        : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("get-profile")]
        public async Task<IActionResult> GetProfile(string email)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    logger.LogError($"{nameof(GetProfile)}: User retrieval went wrong ");
                    return StatusCode(Status500InternalServerError,
                        new ProfileResponse("Error", "User retrieval went wrong"));
                }
                var userName = await userManager.GetUserNameAsync(user);
                var phoneNumber = await userManager.GetPhoneNumberAsync(user);
                return Ok(new ProfileResponse("Success", userName: userName, phoneNumber: phoneNumber));
            }
            catch (Exception exception)
            {
                logger.LogError(exception, nameof(GetProfile));
                return StatusCode(Status500InternalServerError, new ProfileResponse("Error", "An exception occurred")); 
            }
        }
    }
}
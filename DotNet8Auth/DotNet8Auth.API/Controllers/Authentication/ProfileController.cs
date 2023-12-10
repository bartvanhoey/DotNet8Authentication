using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [Route("api/Account")]
    [ApiController]
    public class ProfileController(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
        : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("get-profile")]
        public async Task<IActionResult> GetProfile(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return StatusCode(Status500InternalServerError,
                    new ProfileResponse("Error", "User retrieval went wrong"));

            var userName = await userManager.GetUserNameAsync(user);
            var phoneNumber = await userManager.GetPhoneNumberAsync(user);

            return Ok(new ProfileResponse("Success", userName: userName, phoneNumber: phoneNumber));
        }
    }

    public class ProfileResponse(string? status, string? userName, string? phoneNumber)
    {
        public ProfileResponse(string? status, string? message) : this(status, null, null) => Message = message;

        public string? Status { get; set; } = status;
        public string? Message { get; set; }
        public string? UserName { get; set; } = userName;
        public string? PhoneNumber { get; set; } = phoneNumber;
    }
}
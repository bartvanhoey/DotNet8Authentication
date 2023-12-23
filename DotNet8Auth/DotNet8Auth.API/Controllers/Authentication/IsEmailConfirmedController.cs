using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;


namespace DotNet8Auth.API.Controllers.Authentication
{
    public class IsEmailConfirmedResponse(string? status, bool isEmailConfirmed)
{
    public IsEmailConfirmedResponse(string? status, string? message) 
        : this(status, false) => Message = message;

    public string? Status { get; set; } = status;
    public string? Message { get; set; }
    public bool IsEmailConfirmed { get; set; } = isEmailConfirmed;
}

    [ApiController]
    [Route("api/account")]
    [Authorize]
    public class IsEmailConfirmedController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<UserHasPasswordController> logger) : AuthControllerBase(userManager, configuration)
    {

        [Authorize]
        [HttpGet]
        [Route("is-email-confirmed")]
        public async Task<IActionResult> IsEmailConfirmed()
        {
            try
            {
                var result = ValidateController(logger, nameof(IsEmailConfirmed));
                if (result.IsFailure)
                    return StatusCode(Status500InternalServerError, new IsEmailConfirmedResponse("Error", result.Error?.Message ?? "something went wrong"));

                var email = HttpContext.User.Identity?.Name;
                if (email.IsNullOrWhiteSpace())
                {
                    logger.LogError($"{nameof(IsEmailConfirmed)}: Email was null");
                    return StatusCode(Status500InternalServerError,
                        new IsEmailConfirmedResponse("Error", "Email was null"));
                }

                var user = email == null ? null : await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    logger.LogError($"{nameof(IsEmailConfirmed)}: User retrieval went wrong");
                    return StatusCode(Status500InternalServerError,
                        new IsEmailConfirmedResponse("Error", "User retrieval went wrong"));
                }

                var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);
                return Ok(new IsEmailConfirmedResponse("Success", isEmailConfirmed));
            }
            catch (Exception exception)
            {
                logger.LogError(exception, nameof(IsEmailConfirmed));
                return StatusCode(Status500InternalServerError, new IsEmailConfirmedResponse("Error", "An exception occurred"));
            }
        }

    }


}
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.SetPhoneNumber;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication;

[Route("api/account")]
[ApiController]
public class SetPhoneNumberController(
    UserManager<ApplicationUser> userManager,
    ILogger<SetPhoneNumberController> logger,
    IConfiguration configuration)
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    : AuthControllerBase(userManager, configuration)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [Authorize]
    [HttpPost]
    [Route("set-phone-number")]
    public async Task<IActionResult> SetPhoneNumber([FromBody] SetPhoneNumberInputModel model)
    {
        try
        {
            var validationResult = ValidateInputModel(model, logger, nameof(SetPhoneNumber));
            if (validationResult.IsFailure)
                return StatusCode(Status500InternalServerError,
                    new SetPhoneNumberResponse("Error", validationResult.Error?.Message ?? "something went wrong"));

            var email = HttpContext.User.Identity?.Name;
            if (email.IsNullOrWhiteSpace())
            {
                logger.LogError($"{nameof(SetPhoneNumber)}: Email was null");
                return StatusCode(Status500InternalServerError,
                    new SetPhoneNumberResponse("Error", "email was null"));
            }

            var user = email == null ? null : await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                logger.LogError($"{nameof(SetPhoneNumber)}: User retrieval went wrong");
                return StatusCode(Status500InternalServerError,
                    new SetPhoneNumberResponse("Error", "User retrieval went wrong"));
            }

            var result = await userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
            if (result.Succeeded)
                return Ok(
                    new SetPhoneNumberResponse("Success", userName: user.UserName, phoneNumber: model.PhoneNumber));

            logger.LogError($"{nameof(SetPhoneNumber)}: Update phone number went wrong");
            return StatusCode(Status500InternalServerError,
                new SetPhoneNumberResponse("Error", "update phone number went wrong"));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(SetPhoneNumber));
            return StatusCode(Status500InternalServerError,
                new SetPhoneNumberResponse("Error", "Update phone number went wrong"));
        }
    }
}
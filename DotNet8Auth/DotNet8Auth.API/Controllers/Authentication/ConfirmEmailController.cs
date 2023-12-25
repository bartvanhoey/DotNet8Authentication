using DotNet8Auth.API.Controllers.Authentication.Base;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ConfirmEmail;
using DotNet8Auth.Shared.Models.Authentication.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Text.Encoding;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Microsoft.AspNetCore.WebUtilities.WebEncoders;

namespace DotNet8Auth.API.Controllers.Authentication;

[ApiController]
[Route("api/account")]
public class ConfirmEmailController(UserManager<ApplicationUser> userManager, IHostEnvironment environment, ILogger<ConfirmEmailController> logger, IConfiguration configuration)   
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    : AuthControllerBase(userManager, configuration, environment)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
{
    [HttpPost]
    [Route("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailInputModel model)
    {
        try
        {
            var validationResult = ValidateControllerInputModel(model, logger, nameof(ConfirmEmail));
            if (validationResult.IsFailure) return Nok500<ConfirmEmailResponse>(logger, validationResult.Error?.Message);
            
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null) return Nok500CouldNotFindUser<ConfirmEmailResponse>(logger);

            var code = UTF8.GetString(Base64UrlDecode(model.Code));
            var result = await userManager.ConfirmEmailAsync(user, code);

            return result.Succeeded 
                ? Ok200<ConfirmEmailResponse>("Confirm email successful") 
                : Nok500<ConfirmEmailResponse>(logger, "Confirm email failed! Please try again");
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(ConfirmEmail));
            return Nok500<ConfirmEmailResponse>(logger);
        }
    }
}
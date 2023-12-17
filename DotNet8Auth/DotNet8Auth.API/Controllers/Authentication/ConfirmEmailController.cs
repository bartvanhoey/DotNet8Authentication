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
public class ConfirmEmailController(UserManager<ApplicationUser> userManager, ILogger<ConfirmEmailController> logger) : ControllerBase
{
    [HttpPost]
    [Route("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailInputModel model)
    {
        try
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                logger.LogError($"{nameof(ConfirmEmail)}: User retrieval went wrong ");
                return StatusCode(Status500InternalServerError,
                    new ConfirmEmailResponse { Status = "Error", Message = "User does not exist" });
            }

            var code = UTF8.GetString(Base64UrlDecode(model.Code));
            var result = await userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
                return Ok(new ConfirmEmailResponse { Status = "Success", Message = "Confirm email successful" });

            logger.LogError($"{nameof(ConfirmEmail)}: Confirm email failed! Please try again");
            return StatusCode(Status500InternalServerError,
                new RegisterResponse { Status = "Error", Message = "Confirm email failed! Please try again" });
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(ConfirmEmail));
            return StatusCode(Status500InternalServerError,
                new RegisterResponse { Status = "Error", Message = "Something went wrong" });
        }
    }
}
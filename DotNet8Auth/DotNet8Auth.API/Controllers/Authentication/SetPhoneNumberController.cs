using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.SetPhoneNumber;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [Route("api/account")]
    [ApiController]
    public class SetPhoneNumberController(
        UserManager<ApplicationUser> userManager,
        ILogger<SetPhoneNumberController> logger)
        : ControllerBase
    {
        [Authorize]
        [HttpPost]
        [Route("set-phone-number")]
        public async Task<IActionResult> SetPhoneNumber([FromBody] SetPhoneNumberInputModel model)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    logger.LogError($"{nameof(SetPhoneNumber)}: User retrieval went wrong ");
                    return StatusCode(Status500InternalServerError,
                        new SetPhoneNumberResponse("Error", "User retrieval went wrong"));
                }

                var result = await userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (result.Succeeded)
                    return Ok(new SetPhoneNumberResponse("Success", userName: user.UserName, phoneNumber: model.PhoneNumber));
                
                logger.LogError($"{nameof(SetPhoneNumber)}: Update phone number went wrong ");        
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
}
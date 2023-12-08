using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/Account")]
    public class ConfirmEmailController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }

        [HttpPost]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailInputModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ConfirmEmailResponse { Status = "Error", Message = "User does not exist." });

             var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            var result = await _userManager.ConfirmEmailAsync(user, code);


            if (result.Succeeded)
                return Ok(new ConfirmEmailResponse { Status = "Success", Message = "Confirm email successful." });

            return StatusCode(StatusCodes.Status500InternalServerError, new RegisterResponse { Status = "Error", Message = "Confirm email failed! Please try again." });

        }
    }
}
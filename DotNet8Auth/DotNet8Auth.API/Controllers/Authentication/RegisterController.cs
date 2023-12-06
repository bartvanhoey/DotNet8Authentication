using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/Account")]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            var user = CreateUser();
            if (user == null) return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "CreateUser failed" });;
            user.Email = model.Email;
            user.UserName = model.Email;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        }

        private static ApplicationUser? CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                return null;
            }
        }
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Login;
using DotNet8Auth.Shared.Models.Authentication.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static System.Guid;
using static System.Security.Claims.ClaimTypes;
using static System.String;
using static System.Text.Encoding;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Microsoft.IdentityModel.Tokens.SecurityAlgorithms;

namespace DotNet8Auth.API.Controllers.Authentication
{
    [Route("api/Account")]
    [ApiController]
    public class LoginController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
        : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginInputModel? input)
        {
            try
            {
                if (input is null)
                    return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Login went wrong"));

                var user = await userManager.FindByEmailAsync(input.Email);
                if (user == null)
                    return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Login went wrong"));

                var isPasswordValid = await userManager.CheckPasswordAsync(user, input.Password);
                if (!isPasswordValid)
                    return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Password invalid"));

                var authClaims = new List<Claim>
                {
                    new(Name, user.Email ?? throw new ArgumentNullException()),
                    new(NameIdentifier, user.Id),
                    new(JwtRegisteredClaimNames.Jti, NewGuid().ToString()),
                };

                var userRoles = await userManager.GetRolesAsync(user);
                if (userRoles is { Count: > 0 })
                {
                    authClaims.AddRange(userRoles.Select(userRole => new Claim(Role, userRole)));
                }

                var jwtValidIssuer = configuration["Jwt:ValidIssuer"];
                if (IsNullOrEmpty(jwtValidIssuer))
                    return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Invalid issuer"));
                
                var jwtValidAudience = configuration["Jwt:ValidAudience"];
                if (IsNullOrEmpty(jwtValidAudience))
                    return StatusCode(Status500InternalServerError, new LoginResponse("Error", "Invalid audience"));

                var jwtSecurityKey = configuration["Jwt:SecurityKey"];
                if (IsNullOrEmpty(jwtSecurityKey))
                    return StatusCode(Status500InternalServerError,
                        new LoginResponse("Error", "Security key not configured"));

                var token = new JwtSecurityToken(
                    issuer: jwtValidIssuer,
                    audience: jwtValidAudience,
                    expires: DateTime.UtcNow.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(UTF8.GetBytes(jwtSecurityKey)), HmacSha256)
                );

                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new LoginResponse(accessToken, token.ValidTo));
            }
            catch (Exception)
            {
                return StatusCode(Status500InternalServerError,
                    new LoginResponse(status: "Error", message: "Login went wrong"));
            }
        }


        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterInputModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Email);
            if (userExists != null)
                return StatusCode(Status500InternalServerError,
                    new LoginResponse { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = NewGuid().ToString(),
                UserName = model.Email
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(Status500InternalServerError,
                    new LoginResponse
                    {
                        Status = "Error", Message = "User creation failed! Please check user details and try again."
                    });

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new LoginResponse { Status = "Success", Message = "User created successfully!" });
        }
    }
}
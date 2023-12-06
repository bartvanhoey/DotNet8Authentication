using DotNet8Auth.API.Authentication;
using DotNet8Auth.Shared;
using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotNet8Auth.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public LoginController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] InputModel input)
        {
            try
            {
                if (input is null) throw new ArgumentNullException(nameof(input));

                var user = await _userManager.FindByEmailAsync(input.Email);
                if (user == null) return Unauthorized(); ;

                var isPasswordValid = await _userManager.CheckPasswordAsync(user, input.Password);
                if (!isPasswordValid) return Unauthorized();

                var authClaims = new List<Claim>
                    {
                        new(ClaimTypes.Name, user?.Email ?? throw new ArgumentNullException()),
                        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles != null && userRoles.Count > 0)
                {
                    foreach (var userRole in userRoles) authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var configJwtSecurityKey = _configuration["Jwt:SecurityKey"];
                if (string.IsNullOrEmpty(configJwtSecurityKey)) return Unauthorized();

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configJwtSecurityKey));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:ValidIssuer"],
                    audience: _configuration["Jwt:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                    );

                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new LoginResult { AccessToken = accessToken, ValidTo = token.ValidTo, Successful = true });
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
    }


}
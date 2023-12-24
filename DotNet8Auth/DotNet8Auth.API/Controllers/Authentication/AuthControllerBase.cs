using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Functional;
using DotNet8Auth.Shared.Functional.Errors;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ChangeEmail;
using DotNet8Auth.Shared.Models.Authentication.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static DotNet8Auth.Shared.Functional.Result;
using static Microsoft.AspNetCore.Http.StatusCodes;
using ArgumentNullException = System.ArgumentNullException;

namespace DotNet8Auth.API.Controllers.Authentication;

public class AuthControllerBase(UserManager<ApplicationUser> userManager, IConfiguration configuration) : ControllerBase
{
    protected async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user, string jwtValidIssuer,
        string jwtValidAudience, string jwtSecurityKey)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email ?? throw new InvalidOperationException()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var userRoles = await userManager.GetRolesAsync(user);
        if (userRoles is { Count: > 0 })
        {
            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
        }

        var token = new JwtSecurityToken(
            issuer: jwtValidIssuer,
            audience: jwtValidAudience,
            expires: DateTime.UtcNow.AddSeconds(60),
            claims: authClaims,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecurityKey)),
                SecurityAlgorithms.HmacSha256)
        );
        return token;
    }

    protected IActionResult Nok500<T>(ILogger logger, string? errorMessage = "something went wrong",
        [CallerMemberName] string memberName = "") where T : IControllerResponse
    {
        logger.LogError("{MemberName} : {ErrorMessage}", memberName, errorMessage);
        if (Activator.CreateInstance(typeof(T)) is not IControllerResponse controllerResponse)
        {
            return StatusCode(Status500InternalServerError);
        }

        controllerResponse.Status = "Error";
        controllerResponse.Message = errorMessage;
        return StatusCode(Status500InternalServerError, controllerResponse);
    }
    
    protected IActionResult Nok500<T>(ILogger logger, Exception exception, [CallerMemberName] string memberName = "") where T : IControllerResponse
    {
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        logger.LogError(exception, memberName);
        if (Activator.CreateInstance(typeof(T)) is not IControllerResponse controllerResponse)
            return StatusCode(Status500InternalServerError);

        controllerResponse.Status = "Error";
        controllerResponse.Message = "Something went wrong";
        return StatusCode(Status500InternalServerError, controllerResponse);
    }
    
    

    protected IActionResult Ok200<T>(string message) where T : IControllerResponse
        => Ok(new ConfirmChangeEmailResponse("Success",
            message.IsNullOrWhiteSpace() ? "success" : "Email confirmed successfully"));

    protected IActionResult Nok500<T>(IEnumerable<IdentityError>? errors, ILogger logger,
        [CallerMemberName] string memberName = "") where T : IControllerResponse
    {
        if (errors == null)
        {
            logger.LogError("{MemberName}: Errors is null", memberName);
            return StatusCode(Status500InternalServerError);
        }

        if (Activator.CreateInstance(typeof(T)) is not IControllerResponse controllerResponse)
        {
            logger.LogError("{MemberName}", memberName);
            return StatusCode(Status500InternalServerError);
        }

        controllerResponse.Status = "Error";
        controllerResponse.Errors = errors.Select(x => new ControllerResponseError(x.Code, x.Description));
        foreach (var error in controllerResponse.Errors)
            logger.LogError("{MemberName} : {ErrorCode} - {ErrorDescription}", memberName, error.Code,
                error.Description);

        return StatusCode(Status500InternalServerError, controllerResponse);
    }


    protected Result<ValidateControllerResult> ValidateControllerInputModel<T>(BaseInputModel? input, ILogger<T> logger,
        string methodName)
    {
        if (input is not null) return ValidateController(logger, methodName);
        logger.LogError("{MethodName}: input is null", methodName);
        return Fail<ValidateControllerResult>(new ResultError("input is null"));
    }

    protected Result<ValidateControllerResult> ValidateController<T>(ILogger<T> logger, string methodName)
    {
        var securityKey = configuration["Jwt:SecurityKey"];
        if (IsNullOrEmpty(securityKey))
        {
            logger.LogError("{MethodName}: security key is null", methodName);
            return Fail<ValidateControllerResult>(new ResultError("security key is null"));
        }

        var validIssuer = configuration["Jwt:ValidIssuer"];
        if (IsNullOrEmpty(validIssuer))
        {
            logger.LogError("{MethodName}: valid issuer is null", methodName);
            return Fail<ValidateControllerResult>(new ResultError("valid issuer is null"));
        }

        var originResult = ValidateOrigin(logger, methodName);
        return originResult.IsSuccess
            ? Result.Ok(new ValidateControllerResult(securityKey, validIssuer, originResult.Value.Origin))
            : Fail<ValidateControllerResult>(
                new ResultError(originResult?.Error?.Message ?? "something went wrong"));
    }

    protected Result<ValidateOriginResult> ValidateOrigin<T>(ILogger<T> logger, string methodName)
    {
        var validAudiences = configuration.GetSection("Jwt:ValidAudiences").Get<List<string>>();
        if (validAudiences == null || validAudiences.Count == 0)
        {
            logger.LogError("{MethodName}: audience is null", methodName);
            return Fail<ValidateOriginResult>(new ResultError("audience is null"));
        }

        var origin = HttpContext.Request.Headers.Origin.FirstOrDefault();
        if (origin.IsNotNullOrWhiteSpace() && validAudiences.Contains(origin ?? throw new InvalidOperationException()))
            return Result.Ok(new ValidateOriginResult(origin: origin));

        logger.LogError("{MethodName}: origin is wrong", methodName);
        return Fail<ValidateOriginResult>(new ResultError("origin is wrong"));
    }
}

public class ValidateOriginResult(string origin)
{
    public string Origin { get; } = origin;
}

public class ValidateControllerResult(
    string securityKey,
    string validIssuer,
    string origin)
{
    public string SecurityKey { get; } = securityKey;
    public string ValidIssuer { get; } = validIssuer;
    public string Origin { get; } = origin;
}
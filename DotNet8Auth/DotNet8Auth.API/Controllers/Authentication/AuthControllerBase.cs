using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNet8Auth.Shared.Extensions;
using DotNet8Auth.Shared.Functional;
using DotNet8Auth.Shared.Functional.Errors;
using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static DotNet8Auth.Shared.Functional.Result;

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

    protected Result<ValidateInputModelResult> ValidateInputModel<T>(BaseInputModel? input, ILogger<T> logger,
        string methodName)
    {
        if (input is null)
        {
            logger.LogError("{MethodName}: input is null", methodName);
            return Fail<ValidateInputModelResult>(new ResultError("input is null"));
        }

        var securityKey = configuration["Jwt:SecurityKey"];
        if (IsNullOrEmpty(securityKey))
        {
            logger.LogError("{MethodName}: security key is null", methodName);
            return Fail<ValidateInputModelResult>(new ResultError("security key is null"));
        }

        var validIssuer = configuration["Jwt:ValidIssuer"];
        if (IsNullOrEmpty(validIssuer))
        {
            logger.LogError("{MethodName}: valid issuer is null", methodName);
            return Fail<ValidateInputModelResult>(new ResultError("valid issuer is null"));
        }

        var originResult =  ValidateOrigin(logger, methodName);
        return originResult.IsSuccess
            ? Result.Ok(new ValidateInputModelResult(securityKey, validIssuer, originResult.Value.Origin))
            : Fail<ValidateInputModelResult>(
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
            return  Result.Ok(new ValidateOriginResult(origin: origin));

        logger.LogError("{MethodName}: origin is wrong", methodName);
        return Fail<ValidateOriginResult>(new ResultError("origin is wrong"));
    }
    
    
}

public class ValidateOriginResult(string origin)
{
    public string Origin { get; } = origin;
}

public class ValidateInputModelResult(
    string securityKey,
    string validIssuer,
    string origin)
{
    public string SecurityKey { get; } = securityKey;
    public string ValidIssuer { get; } = validIssuer;
    public string Origin { get; } = origin;
}
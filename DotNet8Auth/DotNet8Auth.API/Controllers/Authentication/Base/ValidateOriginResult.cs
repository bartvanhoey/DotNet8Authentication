namespace DotNet8Auth.API.Controllers.Authentication.Base;

public class ValidateOriginResult(string origin)
{
    public string Origin { get; } = origin;
}
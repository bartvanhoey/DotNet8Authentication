namespace DotNet8Auth.API.Controllers.Authentication;

public class ValidateOriginResult(string origin)
{
    public string Origin { get; } = origin;
}
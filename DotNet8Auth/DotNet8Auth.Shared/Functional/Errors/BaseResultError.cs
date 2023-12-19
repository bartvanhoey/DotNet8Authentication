using DotNet8Auth.Shared.Extensions;

namespace DotNet8Auth.Shared.Functional.Errors;

public abstract class BaseResultError
{
    protected BaseResultError(string message) => Message = message;
    protected BaseResultError() => Message 
        = GetType().Name.Replace("ResultError","").ToSentenceCase();

    public string Message { get; }
}
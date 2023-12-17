using System.Text.Json;

namespace DotNet8Auth.Shared.Extensions;

public static class ObjectExtensions
{
    public static string? ToJson(this object @this)
    {
        return JsonSerializer.Serialize(@this);
    }
}
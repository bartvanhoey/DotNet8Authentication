using System.Text.Json;

namespace DotNet8Auth.Shared.Extensions
{
    public static class StringExtensions
    {
        public static T ConvertJsonTo<T>(this string jsonString)
        {
            return  JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
using System.Text;
using System.Text.Json;

namespace DotNet8Auth.Shared.Extensions
{
    public static class StringExtensions
    {
        public static T? ConvertTo<T>(this string jsonString) => JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        public static string AddUrlParameters(this string url, IDictionary<string, object?> parameters)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url), "Base URL cannot be null or empty.");

            var separator = url.Contains('?') ? "&" : "?";

            var sb = new StringBuilder(url);
            foreach (KeyValuePair<string, object?> parameter in parameters)
            {
                parameter.Deconstruct(out var key, out var value);
                string name = key;
                if (value != null)
                {
                    object value2 = value;
                    sb.Append($"{separator}{key}={Uri.EscapeDataString(value.ToString() ?? "")}");
                    if (separator == "?") separator = "&";
                }
            }

            return sb.ToString();
        }
    }
}
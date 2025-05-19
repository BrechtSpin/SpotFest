using System.Text.RegularExpressions;

namespace Contracts.Utils;
public static class StringExtensions
{
    public static string ToKebabCase(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return string.Empty;

        // Replace whitespace with hyphens
        str = Regex.Replace(str.Trim(), @"\s+", "-");

        // Insert hyphen between camelCase words 
        str = Regex.Replace(str, "(?<!^)([A-Z])", "-$1");

        // Remove invalid characters
        str = Regex.Replace(str, @"[^a-zA-Z0-9\-]", "");

        // remove duplicate -
        str = Regex.Replace(str, "-{2,}", "-");

        return str.ToLowerInvariant();
    }
}
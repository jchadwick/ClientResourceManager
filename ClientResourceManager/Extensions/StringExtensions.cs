using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClientResourceManager
{
    public static class StringExtensions
    {

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static string Join(this IEnumerable<string> values, string separator)
        {
            return string.Join(separator, values);
        }

        public static bool IsFullyQualifiedUrl(this string value)
        {
            return Regex.IsMatch(value, @"^\w://");
        }

        public static bool IsLocalUrl(this string value)
        {
            return Regex.IsMatch(value, @"^~?/");
        }

        public static bool IsUrl(this string value)
        {
            return IsFullyQualifiedUrl(value)
                || IsLocalUrl(value);
        }
    }
}

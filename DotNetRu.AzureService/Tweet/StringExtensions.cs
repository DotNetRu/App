using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetRu.Azure
{
    public static class StringExtensions
    {
        internal static string ConvertToUsualUrl(this string input, List<KeyValuePair<string, string>> replacements)
        {
            var returnString = new StringBuilder(input);
            foreach (var replacement in replacements)
            {
                returnString.Replace(replacement.Key, replacement.Value);
            }

            return Regex.Replace(returnString.ToString(), @"https:\/\/t\.co\/.+$", string.Empty);
        }
    }
}

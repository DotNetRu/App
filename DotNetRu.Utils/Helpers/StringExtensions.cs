using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetRu.Utils.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Strips an url string of http(s):// prefix and a possible trailing / so it is shorter and prettier to display
        /// </summary>
        /// <returns>The original URL</returns>
        /// <param name="url">Url without protocol prefix and trailing slash</param>
        public static string StripUrlForDisplay(this string url)
        {
            if (url == null)
            {
                return null;
            }

            var result = url.Replace("http://", string.Empty);
            result = result.Replace("https://", string.Empty);
            result = result.Replace("www.", string.Empty);
            if (result.EndsWith("/", StringComparison.CurrentCultureIgnoreCase))
            {
                result = result.Remove(result.Length - 1);
            }

            return result.ToLowerInvariant();
        }

        public static string CleanUpTwitter(this string name)
        {
            if (name == null)
            {
                return null;
            }

            var result = name;
            if (name.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || name.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                result = name.Substring(name.LastIndexOf('/') + 1);
            }

            if (result.StartsWith("@", StringComparison.OrdinalIgnoreCase))
            {
                result = result.Remove(0, 1);
            }

            return result;
        }

        public static string GetLastPartOfUrl(this string url)
        {
            if (url == null)
            {
                return null;
            }

            var result = url;

            if (url.LastIndexOf('/') > 0)
            {
                result = url.Substring(url.LastIndexOf('/') + 1);
            }

            return result;
        }

        public static string ToTitleCase(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }

        public static string ConvertToUsualUrl(this string input, Dictionary<string, string> replacements)
        {
            StringBuilder returnString = new StringBuilder(input);
            foreach (var replacment in replacements)
            {
                returnString.Replace(replacment.Key, replacment.Value);
            }
            return Regex.Replace(returnString.ToString(), @"https:\/\/t\.co\/.+$", "");
        }
    }
}


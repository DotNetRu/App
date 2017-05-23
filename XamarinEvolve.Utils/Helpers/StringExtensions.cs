using System;
namespace XamarinEvolve.Utils
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
				return null;

			var result = url.Replace("http://", "");
			result = result.Replace("https://", "");
			result = result.Replace("www.", "");
			if (result.EndsWith("/", StringComparison.CurrentCultureIgnoreCase))
			{
				result = result.Remove(result.Length - 1);
			}

			return result.ToLowerInvariant();
		}

		public static string CleanUpTwitter(this string name)
		{
			if (name == null)
				return null;

			var result = name;
			if (name.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || name.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
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
				return null;

			var result = url;

			if (url.LastIndexOf('/') > 0)
			{
				result = url.Substring(url.LastIndexOf('/') + 1);
			}

			return result;
		}
	}
}


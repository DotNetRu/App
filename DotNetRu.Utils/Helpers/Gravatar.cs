using System.Globalization;

namespace XamarinEvolve.Clients.Portable
{
    /// <summary>
    /// Gravatar interaction.
    /// </summary>
    public static class Gravatar
    {
        const string HttpsUrl = "https://secure.gravatar.com/avatar.php?gravatar_id=";

        /// <summary>
        /// Gets the Gravatar URL.
        /// </summary>
        /// <param name="email">The email of user.</param>
        /// <param name="secure">Use HTTPS?</param>
        /// <param name="size">The Gravatar size.</param>
        /// <param name="rating">The Gravatar rating.</param>
        /// <returns>A gravatar URL.</returns>
        public static string GetURL(string email, int size = 150, 
            string rating = "x") => $"{HttpsUrl}{MD5Core.GetMD5String(email)}&s={size.ToString(CultureInfo.InvariantCulture)}&r={rating}";
		                             
    }
}


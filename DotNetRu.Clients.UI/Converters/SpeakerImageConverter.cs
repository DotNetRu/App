namespace XamarinEvolve.Clients.UI
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using Xamarin.Forms;

    /// <inheritdoc />
    /// <summary>
    /// Used to reaturn the speaker image with caching or default
    /// </summary>
    public class SpeakerImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace((string)value))
                {
                    Debug.WriteLine($"Getting image {value}");

                    return new UriImageSource
                               {
                                   Uri = new Uri((string)value),
                                   CachingEnabled = true,
                                   CacheValidity = TimeSpan.FromDays(3)
                               };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert image to URI: " + ex);
            }

            return ImageSource.FromFile("profile_generic_big.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
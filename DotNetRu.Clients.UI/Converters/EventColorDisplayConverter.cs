namespace DotNetRu.Clients.UI.Converters
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using Xamarin.Forms;

    public class EventColorDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is DateTime))
                {
                    return (Color)Application.Current.Resources["Primary"];
                }

                return DateTime.UtcNow > ((DateTime)value).ToUniversalTime()
                           ? (Color)Application.Current.Resources["Primary"]
                           : Color.FromHex("D3D2D2");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert: " + ex);
            }

            return (Color)Application.Current.Resources["Primary"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
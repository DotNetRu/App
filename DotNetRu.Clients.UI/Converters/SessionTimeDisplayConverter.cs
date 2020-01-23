namespace DotNetRu.Clients.UI.Converters
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using DotNetRu.DataStore.Audit.Models;

    using Xamarin.Forms;

    public class SessionTimeDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is SessionModel session))
                {
                    return string.Empty;
                }

                var startString = session.StartTime.LocalDateTime.ToString("t");
                var endString = session.EndTime.LocalDateTime.ToString("t");

                return $"{startString}–{endString}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert: " + ex);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

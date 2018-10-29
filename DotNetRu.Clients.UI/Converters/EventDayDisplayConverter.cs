namespace DotNetRu.Clients.UI.Converters
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using DotNetRu.Clients.Portable.ApplicationResources;
    using DotNetRu.Clients.Portable.Extensions;

    using Xamarin.Forms;

    public class EventDayDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is DateTime))
                {
                    return string.Empty;
                }

                var dayOfWeek = ((DateTime)value).ToEventTimeZone().DayOfWeek;
                return AppResources.Culture.DateTimeFormat
                    .GetAbbreviatedDayName(
                        dayOfWeek); // DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(dayOfWeek);
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
using DotNetRu.Clients.Portable.ApplicationResources;
using DotNetRu.Clients.Portable.Extensions;
using DotNetRu.Clients.Portable.Model.Extensions;

namespace XamarinEvolve.Clients.UI
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using DotNetRu.DataStore.Audit.Models;

    using Humanizer;

    using Xamarin.Forms;

    public class SessionTimeDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is TalkModel session))
                {
                    return string.Empty;
                }

                return Device.RuntimePlatform == Device.iOS ? session.GetDisplayTime() : session.GetDisplayName();
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

    public class TalkDateDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is TalkModel session))
                {
                    return string.Empty;
                }

                return session.GetDisplayName();
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

    public class EventDateDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is MeetupModel featured))
                {
                    return string.Empty;
                }

                return featured.GetDisplayName();
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

    public class EventTimeDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is MeetupModel featured))
                {
                    return string.Empty;
                }

                return featured.GetDisplayTime();
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

    public class EventDayNumberDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is DateTime))
                {
                    return string.Empty;
                }

                return ((DateTime)value).ToEventTimeZone().Day;
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

    public class HumanizeDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is DateTime)
                {
                    var date = (DateTime)value;
                    if (date.Kind == DateTimeKind.Local)
                    {
                        return date.Humanize(false);
                    }

                    return date.Humanize();
                }
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
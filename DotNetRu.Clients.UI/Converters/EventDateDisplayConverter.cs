namespace DotNetRu.Clients.UI.Converters
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using DotNetRu.Clients.Portable.Model.Extensions;
    using DotNetRu.DataStore.Audit.Models;

    using Xamarin.Forms;
    using Xamarin.Forms.Internals;

    [Preserve]
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

                return featured.GetDisplayDate();
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
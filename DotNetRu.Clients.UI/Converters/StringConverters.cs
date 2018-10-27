using System;
using System.Globalization;

using Xamarin.Forms;

namespace XamarinEvolve.Clients.UI
{
    /// <summary>
    /// Has reminder event text converter.
    /// </summary>
    class HasReminderEventTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            

            return (bool)value ? "Remove from Calendar" : "Add to Calendar";
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Has reminder text converter.
    /// </summary>
    class HasReminderTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            
            return (bool)value ? "Remove from Calendar" : "Add to Calendar";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
       
}


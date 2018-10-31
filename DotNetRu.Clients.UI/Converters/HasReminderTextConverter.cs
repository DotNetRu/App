namespace DotNetRu.Clients.UI.Converters
{
    using System;
    using System.Globalization;

    using Xamarin.Forms;

    public class HasReminderTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return (bool)value ? "Remove from Calendar" : "Add to Calendar";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
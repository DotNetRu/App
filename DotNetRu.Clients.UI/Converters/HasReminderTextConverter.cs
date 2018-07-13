using DotNetRu.Clients.Portable.ApplicationResources;

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
            
            return  AppResources.ResourceManager.GetString((bool)value ? "RemoveFromCalendar" : "AddToCalendar", AppResources.Culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
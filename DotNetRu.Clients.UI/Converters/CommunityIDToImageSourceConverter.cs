using System;
using System.Globalization;
using System.Reflection;
using DotNetRu.DataStore.Audit.Services;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Converters
{
    public class CommunityIDToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return ImageSource.FromResource(
                    stringValue + ".png", typeof(RealmService).GetTypeInfo().Assembly);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

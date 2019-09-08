namespace DotNetRu.Clients.UI.Converters
{
    using System;
    using System.Globalization;
    using DotNetRu.Clients.UI.Localization;
    using Xamarin.Forms;

    public class TranslateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                var localizedValue = AppResources.ResourceManager.GetString(stringValue, AppResources.Culture);

                return localizedValue ?? stringValue;
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

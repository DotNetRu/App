namespace DotNetRu.Clients.UI.Converters
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using Humanizer;

    using Xamarin.Forms;

    public class HumanizeDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is DateTime date)
                {
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
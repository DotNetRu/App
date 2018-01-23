namespace DotNetRu.Clients.UI.Converters
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using DotNetRu.Clients.Portable.Model.Extensions;
    using DotNetRu.DataStore.Audit.Models;

    using Xamarin.Forms;

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
}
namespace XamarinEvolve.Clients.UI
{
    using System;
    using System.Globalization;

    using Xamarin.Forms;

    /// <inheritdoc />
    /// <summary>
    /// Used to return a filled or empty image string
    /// </summary>
    public class IsFilledIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool)value ? $"{parameter}_filled.png" : $"{parameter}_empty.png";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


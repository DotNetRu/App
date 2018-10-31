namespace DotNetRu.Clients.UI.Converters
{
    using System;
    using System.IO;

    using Xamarin.Forms;

    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource imgsrc = null;
            try
            {
                if (value == null)
                {
                    return null;
                }

                byte[] byteArray = value as byte[];

                imgsrc = ImageSource.FromStream(() =>
                    {
                        var ms = new MemoryStream(byteArray) { Position = 0 };
                        return ms;
                    });
            }
            catch (Exception sysExc)
            {
                System.Diagnostics.Debug.WriteLine(sysExc.Message);
            }
            return imgsrc;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

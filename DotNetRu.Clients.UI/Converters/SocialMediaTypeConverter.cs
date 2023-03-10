namespace DotNetRu.Clients.UI.Converters
{
    using System;
    using System.Globalization;
    using DotNetRu.Models.Social;
    using Microsoft.Maui;

    public class SocialMediaTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SocialMediaType socialMediaType)
            {
                if (targetType == typeof(ImageSource))
                {
                    switch (socialMediaType)
                    {
                        case SocialMediaType.Twitter:
                            return "icon_twitter.png";
                        case SocialMediaType.Vkontakte:
                            return "icon_vkontakte.png";
                        default:
                            return "icon_website.png";
                    }
                }
                else if (targetType == typeof(bool))
                {
                    switch (socialMediaType)
                    {
                        case SocialMediaType.Twitter:
                            return false;
                        case SocialMediaType.Vkontakte:
                        default:
                            return true;
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }
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

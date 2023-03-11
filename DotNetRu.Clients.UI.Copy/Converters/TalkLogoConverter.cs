using System;
using System.Globalization;
using System.Linq;
using DotNetRu.DataStore.Audit.Models;
using Microsoft.Maui;

namespace DotNetRu.Clients.UI.Converters
{
    public class TalkLogoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TalkModel talkModel)
            {
                if (talkModel.Speakers.Count() > 1)
                {
                    return talkModel.CommunityLogo;
                }
                else
                {
                    return talkModel.SpeakerAvatar;
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

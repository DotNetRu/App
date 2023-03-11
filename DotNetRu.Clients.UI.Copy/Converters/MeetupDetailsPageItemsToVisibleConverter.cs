using System;
using System.Collections.Generic;
using System.Globalization;
using DotNetRu.Clients.UI.Meetups;
using Microsoft.Maui;

namespace DotNetRu.Clients.UI.Converters
{
    public class MeetupDetailsPageItemsToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection<MeetupDetailsPageItem> collection)
            {
                if (targetType == typeof(bool))
                {
                    return collection.Count > 0;
                }
                else if (targetType == typeof(double))
                {
                    return collection.Count > 0 ? parameter : 0;
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

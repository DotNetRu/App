using System;
using System.Globalization;
using System.Linq;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.Utils.Helpers;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Converters
{
    public class RatingConverter : IValueConverter
    {
        private static readonly string[] defaultValues =
            {
                "Choose a rating", "Not a fan", "It was ok", "Good", "Great", "Love it!"                 
            };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rating = (int)value;

            var values = defaultValues;

            if (parameter is string s)
            {
                values = s.Split(',');
            }

            if (rating >= 0 && rating < values.Length)
            {
                return values[rating];
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Determins if the rating section should be visible
    /// </summary>
    public class RatingVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!FeatureFlags.TalkRatingVisible)
            {
                return false;
            }

            if (!(value is TalkModel talk))
            {
                return false;
            }

            // if it has started or is about to start
            // TODO .First() might not be correct as talk might (?) have several sessions
            return talk.Sessions.First().StartTime.AddMinutes(-15).ToUniversalTime() < DateTime.UtcNow;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

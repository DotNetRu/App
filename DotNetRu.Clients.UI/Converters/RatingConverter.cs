namespace XamarinEvolve.Clients.UI
{
    using System;
    using System.Globalization;

    using Xamarin.Forms;

    using XamarinEvolve.DataObjects;
    using XamarinEvolve.Utils;

    /// <inheritdoc />
    /// <summary>
    /// Rating converter for display text
    /// </summary>
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

            if (!(value is TalkModel session))
            {
                return false;
            }

            if (!session.StartTime.HasValue)
            {
                return false;
            }

            if (session.StartTime.Value == DateTime.MinValue)
            {
                return false;
            }

            // if it has started or is about to start
            return session.StartTime.Value.AddMinutes(-15).ToUniversalTime() < DateTime.UtcNow;
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

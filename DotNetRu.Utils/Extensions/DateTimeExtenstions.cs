namespace XamarinEvolve.Utils.Extensions
{
    using System;
    using System.Diagnostics;

    using NodaTime;

    using XamarinEvolve.Utils.Helpers;

    /// <summary>
    /// The date time extensions.
    /// </summary>
    public static class DateTimeExtenstions
    {
        /// <summary>
        /// The event time zone.
        /// </summary>
        public static readonly DateTimeZone EventTimeZone = DateTimeZoneProviders.Tzdb[EventInfo.TimeZoneName];

        /// <summary>
        /// The to event time zone.
        /// </summary>
        /// <param name="utcDateTime">
        /// The utc date time.
        /// </param>
        /// <param name="isUTC">
        /// The is utc.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime ToEventTimeZone(this DateTime utcDateTime, bool isUTC = true)
        {
            if (utcDateTime == DateTime.MinValue)
            {
                return utcDateTime;
            }

            if (isUTC && utcDateTime.Kind == DateTimeKind.Utc)
            {
                return Instant.FromDateTimeUtc(utcDateTime).InZone(EventTimeZone).ToDateTimeUnspecified();
            }

            return Instant.FromDateTimeOffset(utcDateTime).InZone(EventTimeZone).ToDateTimeUnspecified();
        }

        /// <summary>
        /// The is tba.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsTba(this DateTime date)
        {
            return date.ToEventTimeZone().Year == DateTime.MinValue.Year;
        }

        /// <summary>
        /// The get sort name.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSortName(this DateTime e)
        {
            var start = e.ToEventTimeZone();

            if (DateTime.Today.Year == start.Year)
            {
                if (DateTime.Today.DayOfYear == start.DayOfYear)
                {
                    return "Today";
                }

                if (DateTime.Today.DayOfYear - 1 == start.DayOfYear)
                {
                    return "Yesterday";
                }

                if (DateTime.Today.DayOfYear + 1 == start.DayOfYear)
                {
                    return "Tomorrow";
                }
            }

            var monthDay = start.ToString("M");
            return $"{monthDay}";
        }

        /// <summary>
        /// The get start day.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime GetStartDay(this DateTime date)
        {
            try
            {
                date = date.ToEventTimeZone();
                return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, date.Kind);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert" + ex);
            }

            try
            {
                date = date.ToEventTimeZone(false);
                return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, date.Kind);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to convert" + ex);
            }

            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }
    }
}

namespace DotNetRu.Clients.Portable.Extensions
{
    using System;
    using System.Diagnostics;
    using DotNetRu.Clients.UI.ApplicationResources;

    // using NodaTime;
    public static class DateTimeExtenstions
    {
        // public static readonly DateTimeZone EventTimeZone = DateTimeZoneProviders.Tzdb[EventInfo.TimeZoneName];
        public static DateTime ToEventTimeZone(this DateTime utcDateTime, bool isUTC = true)
        {
            return utcDateTime;

            // if (utcDateTime == DateTime.MinValue)
            // {
            // return utcDateTime;
            // }

            // if (isUTC && utcDateTime.Kind == DateTimeKind.Utc)
            // {
            // return Instant.FromDateTimeUtc(utcDateTime).InZone(EventTimeZone).ToDateTimeUnspecified();
            // }

            // return Instant.FromDateTimeOffset(utcDateTime).InZone(EventTimeZone).ToDateTimeUnspecified();
        }

        public static bool IsTba(this DateTime date)
        {
            return date.ToEventTimeZone().Year == DateTime.MinValue.Year;
        }

        public static string GetSortName(this DateTime e)
        {
            var start = e.ToEventTimeZone();

            if (DateTime.Today.Year == start.Year)
            {
                if (DateTime.Today.DayOfYear == start.DayOfYear)
                {
                    return AppResources.Today;
                }

                if (DateTime.Today.DayOfYear - 1 == start.DayOfYear)
                {
                    return AppResources.Yesterday;
                }

                if (DateTime.Today.DayOfYear + 1 == start.DayOfYear)
                {
                    return AppResources.Tomorrow;
                }
            }

            var monthDay = start.ToString("M");
            return $"{monthDay}";
        }

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

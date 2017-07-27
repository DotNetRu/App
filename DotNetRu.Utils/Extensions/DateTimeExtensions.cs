using System;
using NodaTime;
using System.Diagnostics;

namespace XamarinEvolve.Utils
{
    public static class DateTimeExtenstions
    {
		static readonly DateTimeZone EventTimeZone = DateTimeZoneProviders.Tzdb[EventInfo.TimeZoneName];
        
        public static DateTime ToEventTimeZone(this DateTime utcDateTime, bool isUtc = true)
        {
            if (utcDateTime == DateTime.MinValue)
                return utcDateTime;
            
            if (isUtc && utcDateTime.Kind == DateTimeKind.Utc)
            {
                return Instant.FromDateTimeUtc(utcDateTime)
                .InZone(EventTimeZone)
                .ToDateTimeUnspecified();
            }
            else
            {
                return Instant.FromDateTimeOffset(utcDateTime)
                    .InZone(EventTimeZone)
                    .ToDateTimeUnspecified();
            }
        }

        public static bool IsTBA(this DateTime date)
        {
            
            if (date.ToEventTimeZone().Year == DateTime.MinValue.Year)
                return true;

            return false;
        }

        public static string GetSortName(this DateTime e)
        {
            var start = e.ToEventTimeZone();

            if (DateTime.Today.Year == start.Year)
            {
                if (DateTime.Today.DayOfYear == start.DayOfYear)
                    return $"Today";

                if (DateTime.Today.DayOfYear - 1 == start.DayOfYear)
                    return $"Yesterday";

                if (DateTime.Today.DayOfYear + 1 == start.DayOfYear)
                    return $"Tomorrow";
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


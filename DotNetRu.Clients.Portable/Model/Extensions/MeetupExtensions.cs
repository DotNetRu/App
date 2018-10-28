using XamarinEvolve.Clients.Portable.ApplicationResources;

namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DotNetRu.DataStore.Audit.Models;

    using MvvmHelpers;

    using XamarinEvolve.Clients.Portable;

    public static class MeetupExtensions
    {
        public static IEnumerable<Grouping<string, MeetupModel>> GroupByDate(this IEnumerable<MeetupModel> events)
        {
            return from e in events
                   orderby e.StartTimeOrderBy descending
                   group e by e.GetSortName()
                   into eventGroup
                   select new Grouping<string, MeetupModel>(eventGroup.Key, eventGroup);
        }

        public static string GetSortName(this MeetupModel e)
        {
            if (!e.StartTime.HasValue || !e.EndTime.HasValue)
            {
                return "TBA";
            }

            var start = e.StartTime.Value.ToEventTimeZone();

            if (DateTime.Today.Year == start.Year)
            {
                if (DateTime.Today.DayOfYear == start.DayOfYear)
                {
                    return AppResources.Today;
                }

                if (DateTime.Today.DayOfYear + 1 == start.DayOfYear)
                {
                    return AppResources.Tomorrow;
                }
            }

            // var monthDay = start.ToString("M");
            var year = start.ToString("Y", AppResources.Culture);
            return $"{year}";
        }

        public static string GetDate(this MeetupModel e)
        {
            if (!e.StartTime.HasValue || !e.EndTime.HasValue)
            {
                return "TBA";
            }

            var start = e.StartTime.Value.ToEventTimeZone();

            if (DateTime.Today.Year == start.Year)
            {
                if (DateTime.Today.DayOfYear == start.DayOfYear)
                {
                    return AppResources.Today;
                }

                if (DateTime.Today.DayOfYear + 1 == start.DayOfYear)
                {
                    return AppResources.Tomorrow;
                }
            }

            return start.ToString("MMMM dd, yyyy", AppResources.Culture);
        }

        public static string GetDisplayName(this MeetupModel e)
        {
            if (!e.StartTime.HasValue || !e.EndTime.HasValue || e.StartTime.Value.IsTba())
            {
                return AppResources.ToBeAnnounced;
            }

            var start = e.StartTime.Value.ToEventTimeZone();

            if (e.IsAllDay)
            {
                return AppResources.AllDay;
            }

            var startString = start.ToString("t", AppResources.Culture);
            var end = e.EndTime.Value.ToEventTimeZone();
            var endString = end.ToString("t", AppResources.Culture);

            if (DateTime.Today.Year == start.Year)
            {
                if (DateTime.Today.DayOfYear == start.DayOfYear)
                {
                    return $"{AppResources.Today} {startString}–{endString}";
                }

                if (DateTime.Today.DayOfYear + 1 == start.DayOfYear)
                {
                    return $"{AppResources.Tomorrow} {startString}–{endString}";
                }
            }

            var day = start.DayOfWeek.ToString();
            var monthDay = start.ToString("M", AppResources.Culture);
            return $"{day}, {monthDay}, {startString}–{endString}";
        }

        public static string GetDisplayTime(this MeetupModel e)
        {

            if (!e.StartTime.HasValue || !e.EndTime.HasValue || e.StartTime.Value.IsTba())
            {
                return AppResources.ToBeAnnounced;
            }

            var start = e.StartTime.Value.ToEventTimeZone();


            if (e.IsAllDay)
            {
                return AppResources.AllDay;
            }

            var startString = start.ToString("t", AppResources.Culture);
            var end = e.EndTime.Value.ToEventTimeZone();
            var endString = end.ToString("t", AppResources.Culture);

            return $"{startString}–{endString}";
        }
    }
}



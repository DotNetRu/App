namespace DotNetRu.Clients.Portable.Model.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DotNetRu.Clients.Portable.Extensions;
    using DotNetRu.Clients.UI.ApplicationResources;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.Utils.Helpers;

    using MvvmHelpers;

    public static class MeetupModelExtensions
    {
        public static IEnumerable<Grouping<string, MeetupModel>> GroupByMonth(this IEnumerable<MeetupModel> meetups)
        {
            return from meetup in meetups
                   orderby meetup.StartTime descending
                   group meetup by meetup.GetMonthAndYear()
                   into meetupGroup
                   select new Grouping<string, MeetupModel>(meetupGroup.Key, meetupGroup);
        }

        public static string GetMonthAndYear(this MeetupModel meetupModel)
        {
            if (!meetupModel.StartTime.HasValue)
            {
                return "TBA";
            }

            var start = meetupModel.StartTime.Value.ToEventTimeZone();

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

            var monthAndYear = start.ToString("MMMM yyyy", AppResources.Culture).ToTitleCase();
            return $"{monthAndYear}";
        }

        public static string GetDisplayDate(this MeetupModel meetupModel)
        {
            if (!meetupModel.StartTime.HasValue || !meetupModel.EndTime.HasValue || meetupModel.StartTime.Value.IsTba())
            {
                return AppResources.ToBeAnnounced;
            }

            return meetupModel.StartTime.Value.ToString("d");
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

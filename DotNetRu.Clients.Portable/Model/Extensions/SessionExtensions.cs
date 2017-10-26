namespace XamarinEvolve.Clients.Portable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MvvmHelpers;

    using Xamarin.Forms;

    using XamarinEvolve.DataObjects;
    using XamarinEvolve.Utils;
    using XamarinEvolve.Utils.Extensions;
    using XamarinEvolve.Utils.Helpers;

    public static class SessionExtensions
    {
        public static AppLinkEntry GetAppLink(this TalkModel talkModel)
        {
            var url =
                $"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SessionsSiteSubdirectory.ToLowerInvariant()}/{talkModel.Id}";

            var entry = new AppLinkEntry
                            {
                                Title = talkModel.Title ?? string.Empty,
                                Description = talkModel.Abstract ?? string.Empty,
                                AppLinkUri = new Uri(url, UriKind.RelativeOrAbsolute),
                                IsLinkActive = true
                            };

            if (Device.RuntimePlatform == Device.iOS)
            {
                entry.Thumbnail = ImageSource.FromFile("Icon.png");
            }

            entry.KeyValues.Add("contentType", "Session");
            entry.KeyValues.Add("appName", AboutThisApp.AppName);
            entry.KeyValues.Add("companyName", AboutThisApp.CompanyName);

            return entry;
        }

        public static string GetWebUrl(this TalkModel talkModel)
        {
            return $"http://{AboutThisApp.AppLinksBaseDomain}/{AboutThisApp.SessionsSiteSubdirectory}/#{talkModel.Id}";
        }

        public static string GetIndexName(this TalkModel e)
        {
            if (!e.StartTime.HasValue || !e.EndTime.HasValue || e.StartTime.Value.IsTba())
            {
                return "To be announced";
            }

            var start = e.StartTime.Value.ToEventTimeZone();

            var startString = start.ToString("t");
            var end = e.EndTime.Value.ToEventTimeZone();
            var endString = end.ToString("t");

            var day = start.DayOfWeek.ToString();
            var monthDay = start.ToString("M");
            return $"{day}, {monthDay}, {startString}–{endString}";
        }

        public static string GetSortName(this TalkModel talkModel)
        {
            if (!talkModel.StartTime.HasValue || !talkModel.EndTime.HasValue || talkModel.StartTime.Value.IsTba())
            {
                return "To be announced";
            }

            var start = talkModel.StartTime.Value.ToEventTimeZone();
            var day = start.ToString("M");
            return $"{day}";
        }

        public static string GetDisplayName(this TalkModel talkModel)
        {
            if (!talkModel.StartTime.HasValue || !talkModel.EndTime.HasValue || talkModel.StartTime.Value.IsTba())
            {
                return "TBA";
            }

            var start = talkModel.StartTime.Value.ToEventTimeZone();
            var startString = start.ToString("t");
            var end = talkModel.EndTime.Value.ToEventTimeZone();
            var endString = end.ToString("t");

            if (DateTime.Today.Year == start.Year)
            {
                if (DateTime.Today.DayOfYear == start.DayOfYear)
                {
                    return $"Today {startString}–{endString}";
                }

                if (DateTime.Today.DayOfYear + 1 == start.DayOfYear)
                {
                    return $"Tomorrow {startString}–{endString}";
                }
            }

            var day = start.ToString("M");
            var location = string.Empty;
            if (FeatureFlags.ShowLocationInSessionCell)
            {
                if (talkModel.Room != null)
                {
                    location = $", {talkModel.Room.Name}";
                }
            }

            return $"{day}, {startString}–{endString}{location}";
        }

        public static string GetDisplayTime(this TalkModel talkModel)
        {
            if (!talkModel.StartTime.HasValue || !talkModel.EndTime.HasValue || talkModel.StartTime.Value.IsTba())
            {
                return "TBA";
            }

            var start = talkModel.StartTime.Value.ToEventTimeZone();

            var startString = start.ToString("t");
            var end = talkModel.EndTime.Value.ToEventTimeZone();
            var endString = end.ToString("t");
            var location = string.Empty;
            if (FeatureFlags.ShowLocationInSessionCell)
            {
                if (talkModel.Room != null)
                {
                    location = $", {talkModel.Room.Name}";
                }
            }

            return $"{startString}–{endString}{location}";
        }


        public static IEnumerable<Grouping<string, TalkModel>> FilterAndGroupByDate(this IList<TalkModel> sessions)
        {
            var tba = sessions.Where(s => !s.StartTime.HasValue || !s.EndTime.HasValue || s.StartTime.Value.IsTba());

            var showAllCategories = Settings.Current.ShowAllCategories;
            var filteredCategories = Settings.Current.FilteredCategories;

            var filteredCategoriesList = filteredCategories.Split('|');

            // is not tba
            // has not started or has started and hasn't ended or ended 20 minutes ago
            // filter then by category and filters
            var grouped = (from session in sessions
                           where session.StartTime.HasValue && session.EndTime.HasValue
                                 && !session.StartTime.Value.IsTba()
                                 && (showAllCategories || (session?.Categories.Join(
                                                               filteredCategoriesList,
                                                               category => category.Name,
                                                               filtered => filtered,
                                                               (category, filter) => filter).Any() ?? false))
                           orderby session.StartTimeOrderBy, session.Title
                           group session by session.GetSortName()
                           into sessionGroup
                           select new Grouping<string, TalkModel>(sessionGroup.Key, sessionGroup)).ToList();

            if (tba.Any())
            {
                var tbaFiltered = (from session in tba
                                   where showAllCategories || (session?.Categories.Join(
                                                                    filteredCategoriesList,
                                                                    category => category.Name,
                                                                    filtered => filtered,
                                                                    (category, filter) => filter).Any() ?? false)
                                   select session).ToList();

                grouped.Add(new Grouping<string, TalkModel>("TBA", tbaFiltered));
            }

            return grouped;
        }

        public static IEnumerable<TalkModel> Search(this IEnumerable<TalkModel> sessions, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) return sessions;

            var searchSplit = searchText.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            // search title, then category, then speaker name
            return sessions.Where(
                session => searchSplit.Any(
                    search => session.Haystack.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0));
        }
    }
}


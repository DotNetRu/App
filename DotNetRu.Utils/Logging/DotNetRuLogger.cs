using System;

using DotNetRu.Utils;
using DotNetRu.Utils.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(DotNetRuLogger))]

namespace DotNetRu.Utils
{
    using System.Collections.Generic;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;

    public class DotNetRuLogger : ILogger
    {
        public void TrackPage(string page, string id)
        {
            Analytics.TrackEvent("Page opened", new Dictionary<string, string> { { "Page", page }, { "Id", id } });
        }

        public void TrackTimeSpent(string page, string id, TimeSpan time)
        {
            Analytics.TrackEvent("TrackTimeSpent", new Dictionary<string, string>() {
                { "Page", page },
                { "Id:", id },
                { "Time", time.ToString() }
                });
        }

        public void Track(string trackIdentifier)
        {
            Analytics.TrackEvent(trackIdentifier);
        }

        public void Track(string trackIdentifier, string key, string value)
        {
            Analytics.TrackEvent(trackIdentifier, new Dictionary<string, string> { { key, value } });
        }

        public void Track(string trackIdentifier, IDictionary<string, string> values)
        {
            Analytics.TrackEvent(trackIdentifier, values);
        }

        public static void Report(Exception exception)
        {
            Crashes.TrackError(exception);
        }
    }
}

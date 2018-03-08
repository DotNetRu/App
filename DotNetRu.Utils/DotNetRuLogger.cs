using System;
using System.Collections.Generic;

using DotNetRu.Utils;
using DotNetRu.Utils.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(DotNetRuLogger))]

namespace DotNetRu.Utils
{
    using Microsoft.AppCenter.Analytics;

    public class DotNetRuLogger : ILogger
    {
        public virtual void TrackPage(string page, string id = null)
        {
            Analytics.TrackEvent("TrackPage: " + page + " Id: " + id);
        }

        public virtual void TrackTimeSpent(string page, string id, TimeSpan time)
        {
            Analytics.TrackEvent(
                "TrackTimeSpent: " + page + " Id: " + (id ?? string.Empty)
                + $" Time: {time.TotalMilliseconds} ms");
        }

        public virtual void Track(string trackIdentifier)
        {
            Analytics.TrackEvent("Track: " + trackIdentifier);
        }

        public virtual void Track(string trackIdentifier, string key, string value)
        {
            Analytics.TrackEvent("Track: " + trackIdentifier + " key: " + key + " value: " + value);
        }

        public virtual void Report(Exception exception = null, Severity warningLevel = Severity.Warning)
        {
            Analytics.TrackEvent("Report: " + exception);
        }

        public virtual void Report(
            Exception exception,
            IDictionary<string, string> extraData,
            Severity warningLevel = Severity.Warning)
        {
            Analytics.TrackEvent("Report: " + exception);
        }

        public virtual void Report(
            Exception exception,
            string key,
            string value,
            Severity warningLevel = Severity.Warning)
        {
            Analytics.TrackEvent("Report: " + exception + " key: " + key + " value: " + value);
        }
    }
}

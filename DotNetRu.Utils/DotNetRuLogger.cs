using System;

using DotNetRu.Utils;
using DotNetRu.Utils.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(DotNetRuLogger))]

namespace DotNetRu.Utils
{
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;

    public class DotNetRuLogger : ILogger
    {
        public static void TrackEvent(string message)
        {
            Console.WriteLine(message);
            Analytics.TrackEvent(message);
        }

        public static void Report(Exception exception, string message)
        {
            Console.WriteLine(message + ": " + exception);
            Crashes.TrackError(exception);
        }

        public void TrackPage(string page, string id = null)
        {
            Analytics.TrackEvent("TrackPage: " + page + " Id: " + id);
        }

        public void TrackTimeSpent(string page, string id, TimeSpan time)
        {
            Analytics.TrackEvent(
                "TrackTimeSpent: " + page + " Id: " + (id ?? string.Empty)
                + $" Time: {time.TotalMilliseconds} ms");
        }

        public void Track(string trackIdentifier)
        {
            Analytics.TrackEvent("Track: " + trackIdentifier);
        }

        public void Track(string trackIdentifier, string key, string value)
        {
            Analytics.TrackEvent("Track: " + trackIdentifier + " key: " + key + " value: " + value);
        }

        public void Report(Exception exception)
        {
            Crashes.TrackError(exception);
        }

        public void Report(
            Exception exception,
            string key,
            string value,
            Severity warningLevel = Severity.Warning)
        {
            Analytics.TrackEvent("Report: " + exception + " key: " + key + " value: " + value);
        }
    }
}

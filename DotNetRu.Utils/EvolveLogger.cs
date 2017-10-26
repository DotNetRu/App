using System;
using System.Collections.Generic;
using System.Diagnostics;

using Plugin.GoogleAnalytics;

using Xamarin.Forms;

using XamarinEvolve.Clients.Portable;
using XamarinEvolve.Utils;

[assembly: Dependency(typeof(EvolveLogger))]

namespace XamarinEvolve.Clients.Portable
{
    using XamarinEvolve.Utils.Helpers;

    public class EvolveLogger : ILogger
    {
        private bool googleAnalyticsEnabled = FeatureFlags.GoogleAnalyticsEnabled;

        public EvolveLogger()
        {
            if (FeatureFlags.GoogleAnalyticsEnabled)
            {
                this.SetupGoogleAnalytics();
            }
        }

        private void SetupGoogleAnalytics()
        {
            try
            {
                GoogleAnalytics.Current.Config.TrackingId = ApiKeys.GoogleAnalyticsTrackingId;
                GoogleAnalytics.Current.Config.AppId = AboutThisApp.PackageName;
                GoogleAnalytics.Current.Config.AppName = AboutThisApp.AppName;
                GoogleAnalytics.Current.Config.UseSecure = true;
#if DEBUG
                GoogleAnalytics.Current.Config.Debug = true;
#endif
                GoogleAnalytics.Current.InitTracker();
            }
            catch
            {
                this.googleAnalyticsEnabled = false;
            }
        }

        #region ILogger implementation

        public virtual void TrackPage(string page, string id = null)
        {
            Debug.WriteLine("Logger: TrackPage: " + page.ToString() + " Id: " + id ?? string.Empty);

            if (this.googleAnalyticsEnabled)
            {
                GoogleAnalytics.Current.Tracker.SendView($"{page}Page:{id ?? string.Empty}");
            }
        }

        public virtual void TrackTimeSpent(string page, string id, TimeSpan time)
        {
            Debug.WriteLine(
                "Logger: TrackTimeSpent: " + page.ToString() + " Id: " + (id ?? string.Empty)
                + $" Time: {time.TotalMilliseconds} ms");

            if (this.googleAnalyticsEnabled)
            {
                GoogleAnalytics.Current.Tracker.SendTiming(time, "TimeSpentOnPage", page, id);
            }
        }

        public virtual void Track(string trackIdentifier)
        {
            Debug.WriteLine("Logger: Track: " + trackIdentifier);

            if (this.googleAnalyticsEnabled)
            {
                GoogleAnalytics.Current.Tracker.SendEvent("General", trackIdentifier);
            }
        }

        public virtual void Track(string trackIdentifier, string key, string value)
        {
            Debug.WriteLine("Logger: Track: " + trackIdentifier + " key: " + key + " value: " + value);

            var fullTrackIdentifier = $"{trackIdentifier}-{key}-{@value}";

            if (this.googleAnalyticsEnabled)
            {
                GoogleAnalytics.Current.Tracker.SendEvent("General", fullTrackIdentifier, value);
            }
        }

        public virtual void Report(Exception exception = null, Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("Logger: Report: " + exception);

            if (this.googleAnalyticsEnabled)
            {
                GoogleAnalytics.Current.Tracker.SendException(exception, warningLevel == Severity.Critical);
            }
        }

        public virtual void Report(
            Exception exception,
            IDictionary<string, string> extraData,
            Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("Logger: Report: " + exception);

            if (this.googleAnalyticsEnabled)
            {
                GoogleAnalytics.Current.Tracker.SendException(exception, warningLevel == Severity.Critical);
            }
        }

        public virtual void Report(
            Exception exception,
            string key,
            string value,
            Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("Logger: Report: " + exception + " key: " + key + " value: " + value);

            if (this.googleAnalyticsEnabled)
            {
                GoogleAnalytics.Current.Tracker.SendException(exception, warningLevel == Severity.Critical);
            }
        }

        #endregion
    }
}

using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;
using XamarinEvolve.Clients.Portable;
using System.Diagnostics;
using XamarinEvolve.Utils;
using Plugin.GoogleAnalytics;

[assembly:Dependency(typeof(EvolveLogger))]
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
				SetupGoogleAnalytics();
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
                googleAnalyticsEnabled = false;
            }
        }

        #region ILogger implementation

        public virtual void TrackPage(string page, string id = null)
        {
            Debug.WriteLine("Logger: TrackPage: " + page.ToString() + " Id: " + id ?? string.Empty);

			if (FeatureFlags.HockeyAppEnabled)
			{
#if __ANDROID__
				HockeyApp.Android.Metrics.MetricsManager.TrackEvent($"{page}Page:{id ?? string.Empty}");
#elif __IOS__
				HockeyApp.iOS.BITHockeyManager.SharedHockeyManager?.MetricsManager?.TrackEvent($"{page}Page:{id ?? string.Empty}");
#elif WINDOWS_UWP
                Microsoft.HockeyApp.HockeyClient.Current.TrackEvent($"{page}Page:{id ?? string.Empty}");
                //Microsoft.HockeyApp.HockeyClient.Current.TrackPageView($"{page}Page:{id ?? string.Empty}"); // doesn't show up in HockeyApp!
#endif
            }

            if (googleAnalyticsEnabled)
			{
                GoogleAnalytics.Current.Tracker.SendView($"{page}Page:{id ?? string.Empty}");
            }
        }

		public virtual void TrackTimeSpent(string page, string id, TimeSpan time)
		{
            Debug.WriteLine("Logger: TrackTimeSpent: " + page.ToString() + " Id: " + (id ?? string.Empty) + $" Time: {time.TotalMilliseconds} ms");

            if (FeatureFlags.HockeyAppEnabled)
            {
#if __ANDROID__
#elif __IOS__
#elif WINDOWS_UWP
                Microsoft.HockeyApp.HockeyClient.Current.TrackMetric("TimeSpentOnPage", time.TotalMilliseconds, new Dictionary<string, string> { { "Page", id } });
#endif
            }

            if (googleAnalyticsEnabled)
			{
                GoogleAnalytics.Current.Tracker.SendTiming(time, "TimeSpentOnPage", page, id);
            }
        }

        public virtual void Track(string trackIdentifier)
        {
            Debug.WriteLine("Logger: Track: " + trackIdentifier);

			if (FeatureFlags.HockeyAppEnabled)
			{
#if __ANDROID__
            	HockeyApp.Android.Metrics.MetricsManager.TrackEvent(trackIdentifier);
#elif __IOS__
				HockeyApp.iOS.BITHockeyManager.SharedHockeyManager?.MetricsManager?.TrackEvent(trackIdentifier);
#elif WINDOWS_UWP
                Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(trackIdentifier);
#endif
            }

            if (googleAnalyticsEnabled)
			{
                GoogleAnalytics.Current.Tracker.SendEvent("General", trackIdentifier);
            }
        }

        public virtual void Track(string trackIdentifier, string key, string value)
        {
            Debug.WriteLine("Logger: Track: " + trackIdentifier + " key: " + key + " value: " + value);

            var fullTrackIdentifier = $"{trackIdentifier}-{key}-{@value}";

            if (FeatureFlags.HockeyAppEnabled)
			{
#if __ANDROID__
            	HockeyApp.Android.Metrics.MetricsManager.TrackEvent(fullTrackIdentifier);
#elif __IOS__
				HockeyApp.iOS.BITHockeyManager.SharedHockeyManager?.MetricsManager?.TrackEvent(fullTrackIdentifier);
#elif WINDOWS_UWP
                Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(fullTrackIdentifier, new Dictionary<string, string> { { key, value } });
#endif
            }

            if (googleAnalyticsEnabled)
			{
                GoogleAnalytics.Current.Tracker.SendEvent("General", fullTrackIdentifier, value);
			}
        }
       
        public virtual void Report(Exception exception = null, Severity warningLevel = Severity.Warning)
        {
			Debug.WriteLine("Logger: Report: " + exception);

            if (FeatureFlags.HockeyAppEnabled)
            {
#if __ANDROID__
#elif __IOS__
#elif WINDOWS_UWP
                Microsoft.HockeyApp.HockeyClient.Current.TrackException(exception);
#endif
            }

            if (googleAnalyticsEnabled)
			{
                GoogleAnalytics.Current.Tracker.SendException(exception, warningLevel == Severity.Critical);
			}
        }
        public virtual void Report(Exception exception, IDictionary<string, string> extraData, Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("Logger: Report: " + exception);

            if (FeatureFlags.HockeyAppEnabled)
            {
#if __ANDROID__
#elif __IOS__
#elif WINDOWS_UWP
                Microsoft.HockeyApp.HockeyClient.Current.TrackException(exception, extraData);
#endif
            }

            if (googleAnalyticsEnabled)
			{
                GoogleAnalytics.Current.Tracker.SendException(exception, warningLevel == Severity.Critical);
            }
        }
        public virtual void Report(Exception exception, string key, string value, Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("Logger: Report: " + exception + " key: " + key + " value: " + value);

            if (FeatureFlags.HockeyAppEnabled)
            {
#if __ANDROID__
#elif __IOS__
#elif WINDOWS_UWP
                Microsoft.HockeyApp.HockeyClient.Current.TrackException(exception, new Dictionary<string,string> { { key, value } });
#endif
            }

            if (googleAnalyticsEnabled)
			{
                GoogleAnalytics.Current.Tracker.SendException(exception, warningLevel == Severity.Critical);
			}
        }
#endregion
    }
}

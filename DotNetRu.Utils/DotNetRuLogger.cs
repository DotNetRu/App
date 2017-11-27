using System;
using System.Collections.Generic;
using System.Diagnostics;

using Xamarin.Forms;

using XamarinEvolve.Clients.Portable;

[assembly: Dependency(typeof(DotNetRuLogger))]

namespace XamarinEvolve.Clients.Portable
{
    public class DotNetRuLogger : ILogger
    {
        public virtual void TrackPage(string page, string id = null)
        {
            Debug.WriteLine("Logger: TrackPage: " + page + " Id: " + id);
        }

        public virtual void TrackTimeSpent(string page, string id, TimeSpan time)
        {
            Debug.WriteLine(
                "Logger: TrackTimeSpent: " + page + " Id: " + (id ?? string.Empty)
                + $" Time: {time.TotalMilliseconds} ms");
        }

        public virtual void Track(string trackIdentifier)
        {
            Debug.WriteLine("Logger: Track: " + trackIdentifier);
        }

        public virtual void Track(string trackIdentifier, string key, string value)
        {
            Debug.WriteLine("Logger: Track: " + trackIdentifier + " key: " + key + " value: " + value);
        }

        public virtual void Report(Exception exception = null, Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("Logger: Report: " + exception);
        }

        public virtual void Report(
            Exception exception,
            IDictionary<string, string> extraData,
            Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("Logger: Report: " + exception);
        }

        public virtual void Report(
            Exception exception,
            string key,
            string value,
            Severity warningLevel = Severity.Warning)
        {
            Debug.WriteLine("Logger: Report: " + exception + " key: " + key + " value: " + value);
        }
    }
}

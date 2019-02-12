namespace DotNetRu.Utils.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface ILogger
    {
        void TrackPage(string page, string id);

        void Track(string trackIdentifier);

        void Track(string trackIdentifier, string key, string value);

        void Track(string trackIdentifier, IDictionary<string, string> values);

        void TrackTimeSpent(string page, string id, TimeSpan time);
    }
}

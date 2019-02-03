namespace DotNetRu.Utils.Interfaces
{
    using System;

    public interface ILogger
    {
        void TrackPage(string page, string id);

        void Track(string trackIdentifier);

        void Track(string trackIdentifier, string key, string value);

        void TrackTimeSpent(string page, string id, TimeSpan time);
    }
}

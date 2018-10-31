namespace DotNetRu.Utils.Interfaces
{
    using System;

    public interface ILogger
    {
        void TrackPage(string page, string id = null);

        void Track(string trackIdentifier);

        void Track(string trackIdentifier, string key, string value);

        void Report(Exception exception);

        void Report(Exception exception, string key, string value, Severity warningLevel = Severity.Warning);

        void TrackTimeSpent(string page, string id, TimeSpan time);
    }

    public static class DotNetRuLoggerKeys
    {
        public const string BuyTicket = "BuyTicket";

        public const string CopyPassword = "CopyPassword";

        public const string LoginSuccess = "LoginSuccess";

        public const string LoginFailure = "LoginFailure";

        public const string LoginCancel = "LoginCancel";

        public const string LoginTime = "LoginTime";

        public const string Signup = "Signup";

        public const string ReminderAdded = "ReminderAdded";

        public const string ReminderRemoved = "ReminderRemoved";

        public const string Share = "Share";

        public const string LeaveFeedback = "LeaveFeedback";

        public const string ConferenceFeedback = "ConferenceFeedback";

        public const string ManualSync = "ManualSync";

        public const string LaunchedBrowser = "LaunchedBrowser";

        public const string NavigateToEvolve = "NavigateToEvolve";

        public const string CallVenue = "CallVenue";

        public const string Logout = "Logout";

        public const string GetSyncCode = "GetSyncCode";

        public const string SyncWebToMobile = "SyncWebToMobile";
    }
}
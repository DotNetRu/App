using System;
using System.Collections.Generic;

namespace XamarinEvolve.Clients.Portable
{
  public static class EvolveLoggerKeys
  {
    public const string BuyTicket = "BuyTicket";
    public const string CopyPassword = "CopyPassword";
    public const string LoginSuccess = "LoginSuccess";
    public const string LoginFailure = "LoginFailure";
    public const string LoginCancel = "LoginCancel";
    public const string LoginTime = "LoginTime";
    public const string Signup = "Signup";
    public const string FavoriteAdded = "FavoriteAdded";
    public const string FavoriteRemoved = "FavoriteRemoved";
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

  public enum Severity
  {
    /// <summary>
    /// Warning Severity
    /// </summary>
    Warning,

    /// <summary>
    /// Error Severity, you are not expected to call this from client side code unless you have disabled unhandled exception handling.
    /// </summary>
    Error,

    /// <summary>
    /// Critical Severity
    /// </summary>
    Critical
  }

  public interface ILogger
  {
    void TrackPage(string page, string id = null);
    void Track(string trackIdentifier);
    void Track(string trackIdentifier, string key, string value);
    void Report(Exception exception = null, Severity warningLevel = Severity.Warning);
    void Report(Exception exception, IDictionary<string, string> extraData, Severity warningLevel = Severity.Warning);
    void Report(Exception exception, string key, string value, Severity warningLevel = Severity.Warning);
    void TrackTimeSpent(string page, string id, TimeSpan time);
  }
}


using System;
using System.Threading.Tasks;
using Plugin.Calendars.Abstractions;
using Plugin.Calendars;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using Xamarin.Essentials;

namespace DotNetRu.Clients.UI.News
{
    public static class CalendarService
    {
        private const string CalendarName = "DotNetRu";

        private static string GetEventIDKey(string eventID) => "calendar_" + eventID;

        private static void SaveExternalEventID(string eventID, string externalEventID) => Preferences.Set(GetEventIDKey(eventID), externalEventID);

        private static string GetExternalEventID(string eventID) => Preferences.Get(GetEventIDKey(eventID), string.Empty);

        private static void RemoveExternalEventID(string eventID) => Preferences.Set(GetEventIDKey(eventID), string.Empty);

        public static string CalendarID
        {
            get => Preferences.Get(nameof(CalendarID), string.Empty);
            set => Preferences.Set(nameof(CalendarID), value);
        }

        public static bool WasCalendarUsed
        {
            get => Preferences.Get(nameof(WasCalendarUsed), false);
            set => Preferences.Set(nameof(WasCalendarUsed), value);
        }

        public static async Task<bool> HasReminderAsync(string id)
        {
            if (!WasCalendarUsed)
            {
                return false;
            }

            var hasPermissions = await GetCalendarPermissionsAsync();
            if (!hasPermissions)
            {
                return false;
            }

            var externalId = GetExternalEventID(id);
            if (string.IsNullOrWhiteSpace(externalId))
            {
                return false;
            }

            try
            {
                var calEvent = await CrossCalendars.Current.GetEventByIdAsync(externalId);
                return calEvent != null;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                RemoveExternalEventID(id);
            }
            return false;
        }

        public static async Task<bool> RemoveCalendarEventAsync(string eventID)
        {
            var hasPermissions = await GetCalendarPermissionsAsync();
            if (!hasPermissions)
            {
                return false;
            }

            try
            {
                var calendar = await GetOrCreateCalendarAsync();

                var externalEventID = GetExternalEventID(eventID);

                var calendarEvent = await CrossCalendars.Current.GetEventByIdAsync(externalEventID);
                await CrossCalendars.Current.DeleteEventAsync(calendar, calendarEvent);

                RemoveExternalEventID(eventID);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return false;
            }
            return true;
        }

        public static async Task<bool> AddCalendarEventAsync(string eventID, CalendarEvent calendarEvent)
        {
            var hasPermissions = await GetCalendarPermissionsAsync();
            if (!hasPermissions)
            {
                return false;
            }

            try
            {
                var calendar = await GetOrCreateCalendarAsync();

                await CrossCalendars.Current.AddOrUpdateEventAsync(calendar, calendarEvent);

                SaveExternalEventID(eventID, calendarEvent.ExternalID);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return false;
            }
            return true;
        }


        public static async Task<bool> GetCalendarPermissionsAsync()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Calendar);
            if (status != PermissionStatus.Granted)
            {
                var request = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Calendar);
                if (!request.ContainsKey(Permission.Calendar) || request[Permission.Calendar] != PermissionStatus.Granted)
                {
                    Analytics.TrackEvent("Calendar Permission Denied");
                    return false;
                }
            }

            return true;
        }


        public static async Task<Calendar> GetOrCreateCalendarAsync()
        {

            var id = CalendarID;
            if (!string.IsNullOrWhiteSpace(id))
            {
                try
                {
                    var calendar = await CrossCalendars.Current.GetCalendarByIdAsync(id);
                    if (calendar != null)
                    {
                        return calendar;
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }

            // if for some reason the calendar does not exist then simply create a new one.
            if (Device.RuntimePlatform == Device.Android)
            {
                // On android it is really hard to delete a calendar made by an app, so just add to default calendar.
                try
                {
                    var calendars = await CrossCalendars.Current.GetCalendarsAsync();
                    foreach (var calendar in calendars)
                    {
                        // find first calendar we can add stuff to
                        if (!calendar.CanEditEvents)
                        {
                            continue;
                        }

                        CalendarID = calendar.ExternalID;
                        return calendar;
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                // try to find app if already uninstalled for some reason
                try
                {
                    var calendars = await CrossCalendars.Current.GetCalendarsAsync();
                    foreach (var calendar in calendars)
                    {
                        // find first calendar we can add stuff to
                        if (calendar.CanEditEvents && calendar.Name == CalendarName)
                        {
                            CalendarID = calendar.ExternalID;
                            return calendar;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }

            var appCalendar = new Calendar
            {
                Color = "#5c7cbc",
                Name = CalendarName
            };

            try
            {
                await CrossCalendars.Current.AddOrUpdateCalendarAsync(appCalendar);
                CalendarID = appCalendar.ExternalID;
                return appCalendar;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return null;
        }
    }
}


using System;

namespace DotNetRu.Clients.UI.News
        private const string CalendarName = "DotNetRu";

        private static string GetEventIDKey(string eventID) => "calendar_" + eventID;

        private static void SaveExternalEventID(string eventID, string externalEventID) => Preferences.Set(GetEventIDKey(eventID), externalEventID);

        public static bool WasCalendarUsed
            if (!WasCalendarUsed)
            {
                return false;
            }

            var hasPermissions = await GetCalendarPermissionsAsync();
            {
            }
            {
            }

                RemoveExternalEventID(eventID);
                Crashes.TrackError(ex);
            {
            }

                await CrossCalendars.Current.AddOrUpdateEventAsync(calendar, calendarEvent);

                SaveExternalEventID(eventID, calendarEvent.ExternalID);
                Crashes.TrackError(ex);
                    Analytics.TrackEvent("Calendar Permission Denied");

            var id = CalendarID;
                    {
                        return calendar;
                    }

            // if for some reason the calendar does not exist then simply create a new one.
            if (Device.RuntimePlatform == Device.Android)
                // On android it is really hard to delete a calendar made by an app, so just add to default calendar.
                try
                        // find first calendar we can add stuff to
                        if (!calendar.CanEditEvents)
                        {
                        }
                // try to find app if already uninstalled for some reason
                try
                        // find first calendar we can add stuff to
                        if (calendar.CanEditEvents && calendar.Name == CalendarName)
            {
                Color = "#5c7cbc",
                Name = CalendarName
            };
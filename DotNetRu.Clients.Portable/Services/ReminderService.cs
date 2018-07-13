﻿using System.Linq;

namespace DotNetRu.Clients.Portable.Services
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using DotNetRu.Clients.Portable.Helpers;
    using DotNetRu.Clients.Portable.Interfaces;
    using DotNetRu.Utils.Helpers;

    using FormsToolkit;

    using Plugin.Calendars;
    using Plugin.Calendars.Abstractions;
    using Plugin.Permissions;
    using Plugin.Permissions.Abstractions;

    using Xamarin.Forms;

    //using Settings = XamarinEvolve.Clients.Portable.Settings;

    public static class ReminderService
    {
        //public static async Task<bool> HasReminderAsync(string id)
        //{
        //    if (!Settings.Current.HasSetReminder)
        //    {
        //        return false;
        //    }

        //    var ready = await CheckPermissionsGetCalendarAsync(false);
        //    if (!ready) return false;

        //    var externalId = Settings.Current.GetEventId(id);
        //    if (string.IsNullOrWhiteSpace(externalId)) return false;

        //    try
        //    {
        //        var calEvent = await CrossCalendars.Current.GetEventByIdAsync(externalId);
        //        return calEvent != null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("MeetupModel has an Id, but doesn't exist, removing" + ex);
        //        Settings.Current.RemoveReminderId(id);

        //    }

        //    return false;
        //}

        public static async Task<bool> AddSessionAsync(CalendarEvent calEvent)
        {
            var ready = await CheckPermission();
            if (!ready)
                return false;

            try
            {
                var calendar = await GetOrCreateCalendarAsync();
                await CrossCalendars.Current.AddOrUpdateEventAsync(calendar, calEvent);
            }
            catch (Exception)
            {

                return false;
            }
            

            return true;

            //var ready = await CheckPermissionsGetCalendarAsync();
            //if (!ready) return false;

            //try
            //{
            //    var evolveCal = await GetOrCreateEvolveCalendarAsync();

            //    // Create event and then create the reminder!
            //    await CrossCalendars.Current.AddOrUpdateEventAsync(evolveCal, calEvent);
            //    await CrossCalendars.Current.AddEventReminderAsync(
            //        calEvent,
            //        new CalendarEventReminder
            //        {
            //            Method = CalendarReminderMethod.Default,
            //            TimeBefore = TimeSpan.FromMinutes(20)
            //        });
            //    Settings.Current.SaveReminderId(id, calEvent.ExternalID);

            //    if (!Settings.Current.HasSetReminder)
            //    {
            //        MessagingService.Current.SendMessage<MessagingServiceAlert>(
            //            MessageKeys.Message,
            //            new MessagingServiceAlert
            //            {
            //                Title = "Reminder Added",
            //                Message =
            //                        $"Reminder has been added. Please check the {evolveCal.Name} calendar.",
            //                Cancel = "OK"
            //            });
            //    }

            //    Settings.Current.HasSetReminder = true;
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("Unable to create event: " + ex);
            //    MessagingService.Current.SendMessage<MessagingServiceAlert>(
            //        MessageKeys.Message,
            //        new MessagingServiceAlert
            //        {
            //            Title = "MeetupModel Creation",
            //            Message =
            //                    "Unable to create calendar event, please check calendar app and try again.",
            //            Cancel = "OK"
            //        });
            //    return false;
            //}

            //return true;
        }


        public static async Task<bool> DeleteSessionAsync(CalendarEvent calEvent)
        {
            var ready = await CheckPermission();
            if (!ready)
                return false;

            try
            {
                var calendar = await GetOrCreateCalendarAsync();
                await CrossCalendars.Current.DeleteEventAsync(calendar, calEvent);
            }
            catch (Exception)
            {

                return false;
            }


            return true;
        }

        //public static async Task<bool> RemoveReminderAsync(string id)
        //{
        //    var ready = await CheckPermissionsGetCalendarAsync();
        //    if (!ready) return false;


        //    try
        //    {
        //        var evolveCal = await GetOrCreateEvolveCalendarAsync();
        //        var externalId = Settings.Current.GetEventId(id);
        //        var calEvent = await CrossCalendars.Current.GetEventByIdAsync(externalId);
        //        await CrossCalendars.Current.DeleteEventAsync(evolveCal, calEvent);
        //        Settings.Current.RemoveReminderId(id);
        //        Settings.Current.HasSetReminder = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Unable to delete event: " + ex);
        //        MessagingService.Current.SendMessage<MessagingServiceAlert>(
        //            MessageKeys.Message,
        //            new MessagingServiceAlert
        //            {
        //                Title = "MeetupModel Deletion",
        //                Message =
        //                        "Unable to delete calendar event, please check calendar app and try again.",
        //                Cancel = "OK"
        //            });
        //        return false;
        //    }

        //    return true;
        //}


        //static async Task<bool> CheckPermissionsGetCalendarAsync(bool alert = true)
        //{
        //    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Calendar);
        //    if (status != PermissionStatus.Granted)
        //    {
        //        var request = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Calendar);
        //        if (!request.ContainsKey(Permission.Calendar)
        //            || request[Permission.Calendar] != PermissionStatus.Granted)
        //        {
        //            if (alert)
        //            {
        //                if (Device.RuntimePlatform == Device.iOS)
        //                {
        //                    MessagingService.Current.SendMessage<MessagingServiceQuestion>(
        //                        MessageKeys.Question,
        //                        new MessagingServiceQuestion
        //                        {
        //                            Title = "Calendar Permission",
        //                            Question =
        //                                    $"Unable to set reminders as the Calendar permission was not granted. Please go into Settings and turn on Calendars for {AboutThisApp.AppName}.",
        //                            Positive = "Settings",
        //                            Negative = "Maybe Later",
        //                            OnCompleted = (result) =>
        //                                {
        //                                    if (result)
        //                                    {
        //                                        DependencyService.Get<IPushNotifications>()
        //                                            .OpenSettings();
        //                                    }
        //                                }
        //                        });
        //                }
        //                else
        //                {
        //                    MessagingService.Current.SendMessage<MessagingServiceAlert>(
        //                        MessageKeys.Message,
        //                        new MessagingServiceAlert
        //                        {
        //                            Title = "Calendar Permission",
        //                            Message =
        //                                    "Unable to set reminders as the Calendar permission was not granted, please check your settings and try again.",
        //                            Cancel = "OK"
        //                        });
        //                }
        //            }

        //            return false;
        //        }
        //    }

        //    var currentCalendar = await GetOrCreateEvolveCalendarAsync();

        //    if (currentCalendar == null)
        //    {
        //        if (alert)
        //        {
        //            MessagingService.Current.SendMessage<MessagingServiceAlert>(
        //                MessageKeys.Message,
        //                new MessagingServiceAlert
        //                {
        //                    Title = "No Calendar",
        //                    Message =
        //                            $"We were unable to get or create the {AboutThisApp.AppName} calendar, please check your calendar app and try again.",
        //                    Cancel = "OK"
        //                });
        //        }

        //        return false;
        //    }

        //    return true;
        //}

        private static async Task<bool> CheckPermission(bool alert = true)
        {
            if (Device.RuntimePlatform != Device.Android)
            {
                return true;
            }

            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Calendar);
            if (status != PermissionStatus.Granted)
            {
                var request = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Calendar);
                if (!request.ContainsKey(Permission.Calendar) ||
                    request[Permission.Calendar] != PermissionStatus.Granted)
                {
                    if (alert)
                    {
                        MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message,
                            new MessagingServiceAlert
                            {
                                Title = "Calendar Permission",
                                Message =
                                    "Unable to set reminders as the Calendar permission was not granted, please check your settings and try again.",
                                Cancel = "OK"
                            });
                    }

                    return false;
                }
            }

            return true;
        }


        private static async Task<Calendar> GetOrCreateCalendarAsync()
        {

            var id = "";//Settings.Current.EventCalendarId;
            if (!string.IsNullOrWhiteSpace(id))
            {
                try
                {
                    var calendar = await CrossCalendars.Current.GetCalendarByIdAsync(id);
                    if (calendar != null) return calendar;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unable to get calendar... odd as we created it already: " + ex);

                }

            }

            // if for some reason the calendar does not exist then simply create a enw one.
            if (Device.RuntimePlatform == Device.Android)
            {
                // On android it is really hard to delete a calendar made by an app, so just add to default calendar.
                try
                {
                    var calendars = await CrossCalendars.Current.GetCalendarsAsync();
                    foreach (var calendar in calendars)
                    {
                        // find first calendar we can add stuff to
                        if (!calendar.CanEditEvents) continue;

                        //Settings.Current.EventCalendarId = calendar.ExternalID;
                        return calendar;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unable to get calendars..." + ex);
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
                        if (calendar.CanEditEvents && calendar.Name == AboutThisApp.AppName)
                        {
                            //Settings.Current.EventCalendarId = calendar.ExternalID;
                            return calendar;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unable to get calendars..." + ex);
                }
            }

            var conferenceCalendar = new Calendar
            {
                Color = ((Color)Application.Current.Resources["Primary"]).ToHex(),
                Name = AboutThisApp.AppName
            };
            try
            {
                await CrossCalendars.Current.AddOrUpdateCalendarAsync(conferenceCalendar);
                //Settings.Current.EventCalendarId = conferenceCalendar.ExternalID;
                return conferenceCalendar;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to create calendar..." + ex);
            }

            return null;
        }
    }
}


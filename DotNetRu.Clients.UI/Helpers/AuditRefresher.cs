using System;
using DotNetRu.DataStore.Audit.Services;
using DotNetRu.Utils;
using DotNetRu.Utils.Helpers;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Helpers
{
    public class AuditRefresher
    {
        private static readonly object locker = new object();
        private static AuditRefresher instance;

        public static AuditRefresher Instance
        {
            get
            {
                lock (locker)
                {
                    return instance ?? (instance = new AuditRefresher());
                }
            }
        }

        private AuditRefresher()
        {
        }        

        public static void Refresh(UpdateResults updateResults)
        {
            try
            {
                if (updateResults.HasFlag(UpdateResults.Speakers))
                {
                    MessagingCenter.Send(Instance, MessageKeys.SpeakersChanged);
                }
                if (updateResults.HasFlag(UpdateResults.Meetups))
                {
                    MessagingCenter.Send(Instance, MessageKeys.MeetupsChanged);
                }
                if (updateResults.HasFlag(UpdateResults.Friends))
                {
                    MessagingCenter.Send(Instance, MessageKeys.FriendsChanged);
                }
            }
            catch (Exception e)
            {
                DotNetRuLogger.Report(e);
            }
        }
    }
}

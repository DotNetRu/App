namespace DotNetRu.DataStore.Audit.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DotNetRu.DataStore.Audit.Entities;
    using DotNetRu.DataStore.Audit.Extensions;
    using DotNetRu.DataStore.Audit.Helpers;
    using DotNetRu.DataStore.Audit.Models;

    public static class MeetupService
    {
        private static IEnumerable<MeetupModel> meetups;

        public static IEnumerable<MeetupModel> Meetups => meetups ?? (meetups = GetMeetups());

        public static MeetupModel GetMeetup(string talkID)
        {
            return Meetups.SingleOrDefault(x => x.TalkIDs.Contains(talkID));
        }

        private static IEnumerable<MeetupModel> GetMeetups()
        {
            var meetupEntities = ParseHelper.ParseXml<MeetupEntity>("Meetups");
            return meetupEntities.Select(x => x.ToModel());
        }        
    }
}

namespace DotNetRu.DataStore.Audit.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DotNetRu.DataStore.Audit.Extensions;
    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.RealmModels;

    public static class MeetupService
    {
        private static IEnumerable<MeetupModel> meetups;

        public static IEnumerable<MeetupModel> Meetups => meetups ?? (meetups = GetMeetups());

        public static MeetupModel GetMeetup(string talkID)
        {
            return Meetups.SingleOrDefault(x => x.TalkIDs.Contains(talkID));
        }

        public static IEnumerable<MeetupModel> GetMeetups(string friendID)
        {
            return Meetups.Where(meetup => meetup.FriendIDs.Contains(friendID));
        }

        private static IEnumerable<MeetupModel> GetMeetups()
        {
            var meetupEntities = RealmService.AuditRealm.All<Meetup>().ToList();
            return meetupEntities.Select(x => x.ToModel());
        }        
    }
}

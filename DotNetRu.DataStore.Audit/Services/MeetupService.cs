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

        private static IEnumerable<MeetupModel> GetMeetups()
        {
            var meetupEntities = RealmService.AuditRealm.All<Meetup>().ToList();
            return meetupEntities.Select(x => x.ToModel());
        }        
    }
}

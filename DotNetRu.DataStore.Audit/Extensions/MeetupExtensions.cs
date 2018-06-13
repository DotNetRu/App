namespace DotNetRu.DataStore.Audit.Extensions
{
    using System.Linq;

    using DotNetRu.DataStore.Audit.Models;
    using DotNetRu.DataStore.Audit.RealmModels;

    public static class MeetupExtensions
    {
        public static MeetupModel ToModel(this Meetup meetup)
        {
            return new MeetupModel
                       {
                           CommunityID = meetup.CommunityId,
                           Description = meetup.Name,
                           IsAllDay = true,
                           Title = meetup.Name,
                           StartTime = meetup.Date.LocalDateTime,
                           EndTime = meetup.Date.LocalDateTime,
                           Venue = meetup.Venue.ToModel(),
                           Talks = meetup.Talks.Select(x => x.ToModel()),
                           Friends = meetup.Friends.Select(x => x.ToModel())
                       };
        }
    }
}

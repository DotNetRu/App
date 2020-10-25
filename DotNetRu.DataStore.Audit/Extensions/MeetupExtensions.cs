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
                Id = meetup.Id,
                CommunityID = meetup.CommunityId,
                Description = meetup.Name,
                IsAllDay = false,
                Title = meetup.Name,
                StartTime = meetup.Sessions.First().StartTime.DateTime,
                EndTime = meetup.Sessions.Last().EndTime.DateTime,
                Venue = meetup.Venue?.ToModel(),
                Sessions = meetup.Sessions.Select(x => x.ToModel()),
                Friends = meetup.Friends.Select(x => x.ToModel())
            };
        }
    }
}

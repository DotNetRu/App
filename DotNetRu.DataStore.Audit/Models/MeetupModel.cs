namespace DotNetRu.DataStore.Audit.Models
{
    using System;
    using System.Collections.Generic;    

    public class MeetupModel
    {
        public string Id { get; set; }

        public string CommunityID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool IsAllDay { get; set; }

        public VenueModel Venue { get; set; }

        public IEnumerable<FriendModel> Friends { get; set; }

        public IEnumerable<TalkModel> Talks { get; set; }
    }
}

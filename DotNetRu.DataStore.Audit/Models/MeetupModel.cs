namespace DotNetRu.DataStore.Audit.Models
{
    using System;
    using System.Collections.Generic;    

    public class MeetupModel : BaseModel
    {
        public string CommunityID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool IsAllDay { get; set; }

        public string VenueID { get; set; }

        public virtual FriendModel FriendModel { get; set; }

        public bool HasSponsor => this.FriendModel != null;

        public IEnumerable<string> TalkIDs { get; set; }
    }
}

using DotNetRu.Clients.Portable.Model;
using DotNetRu.DataStore.Audit.Models;

namespace DotNetRu.Clients.UI.Meetups
{
    public class MeetupDetailsPageItem
    {
        public MeetupDetailsItemType ItemType { get; set; }

        public FriendModel Friend { get; set; }

        public SessionModel Session { get; set; }
    }
}

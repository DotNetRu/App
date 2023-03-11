using System;
using Microsoft.Maui;

namespace DotNetRu.Clients.UI.Meetups
{
    public class MeetupDetailsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate VenueTemplate { get; set; }

        public DataTemplate FriendTemplate { get; set; }

        public DataTemplate SessionTemplate { get; set; }

        public DataTemplate CalendarTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var aboutPageItem = item as MeetupDetailsPageItem;

            switch (aboutPageItem.ItemType)
            {
                case MeetupDetailsItemType.Friend:
                    return FriendTemplate;
                case MeetupDetailsItemType.Session:
                    return SessionTemplate;
                case MeetupDetailsItemType.Venue:
                    return VenueTemplate;
                case MeetupDetailsItemType.Calendar:
                    return CalendarTemplate;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

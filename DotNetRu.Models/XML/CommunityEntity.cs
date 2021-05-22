namespace DotNetRu.Models.XML
{
    using System.Xml.Serialization;

    [XmlType("Community")]
    public class CommunityEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string TimeZone { get; set; }

        public string VkUrl { get; set; }

        public string TwitterUrl { get; set; }

        public string TelegramChannelUrl { get; set; }

        public string TelegramChatUrl { get; set; }

        public string TimePadUrl { get; set; }

        public string MeetupComUrl { get; set; }
    }
}

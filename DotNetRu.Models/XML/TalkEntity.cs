namespace DotNetRu.Models.XML
{
    using System.Xml.Serialization;

    [XmlType("Talk")]
    public class TalkEntity
    {
        public string Id { get; set; }

        [XmlArrayItem("SpeakerId", IsNullable = false)]
        public string[] SpeakerIds { get; set; }

        [XmlArrayItem("TalkId", IsNullable = false)]
        public string[] SeeAlsoTalkIds { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string CodeUrl { get; set; }

        public string SlidesUrl { get; set; }

        public string VideoUrl { get; set; }
    }
}


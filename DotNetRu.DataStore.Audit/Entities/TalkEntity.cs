namespace DotNetRu.DataStore.Audit.Entities
{
    using System.Xml.Serialization;

    [XmlType("Talk")]
    public class TalkEntity
    {
        public string Id { get; set; }

        [XmlArrayItem("SpeakerId", IsNullable = false)]
        public string[] SpeakerIds { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [XmlArrayItem("TalkId", IsNullable = true)]
        public string[] SeeAlsoTalkIds { get; set; }

        public string CodeUrl { get; set; }

        public string SlidesUrl { get; set; }

        public string VideoUrl { get; set; }
    }
}


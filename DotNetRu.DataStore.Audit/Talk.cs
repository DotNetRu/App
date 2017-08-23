﻿using System.Xml.Serialization;

namespace DotNetRu.DataStore.Audit
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Talk
    {
 
        public string Id { get; set; }

        [XmlArrayItem("SpeakerId", IsNullable = false)]
        public string[] SpeakerIds { get; set; }

 
        public string Title { get; set; }

 
        public string Description { get; set; }
    }
}
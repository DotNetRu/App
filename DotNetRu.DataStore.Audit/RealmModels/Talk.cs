namespace DotNetRu.DataStore.Audit.RealmModels
{
    using System.Collections.Generic;

    using Realms;

    public class Talk : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public IList<Speaker> Speakers { get; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string CodeUrl { get; set; }

        public string SlidesUrl { get; set; }

        public string VideoUrl { get; set; }
    }
}


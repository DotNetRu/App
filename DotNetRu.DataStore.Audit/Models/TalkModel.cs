namespace DotNetRu.DataStore.Audit.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using DotNetRu.DataStore.Audit.Services;
    using Xamarin.Forms;

    public class TalkModel : IIdentified
    {
        private const string Delimiter = "|";

        private readonly IList<string> seeAlsoTalksIds;

        private string haystack;

        public TalkModel(IList<string> seeAlsoTalksIds)
        {
            this.seeAlsoTalksIds = seeAlsoTalksIds == null ? new List<string>() : new List<string>(seeAlsoTalksIds);

            this.Speakers = new List<SpeakerModel>();
            this.Categories = new List<Category>();
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string ShortTitle { get; set; }

        public string Abstract { get; set; }

        public IEnumerable<SpeakerModel> Speakers { get; set; }

        public IEnumerable<TalkModel> SeeAlsoTalks => CachedModelsProvider<TalkModel>.Get(seeAlsoTalksIds);

        public ICollection<Category> Categories { get; set; }

        public string PresentationUrl { get; set; }

        public string VideoUrl { get; set; }

        public string CodeUrl { get; set; }

        public string Haystack
        {
            get
            {
                if (this.haystack != null)
                {
                    return this.haystack;
                }

                var builder = new StringBuilder();
                builder.Append(Delimiter);
                builder.Append(this.Title);
                builder.Append(Delimiter);
                if (this.Categories != null)
                {
                    foreach (var c in this.Categories)
                    {
                        builder.Append($"{c.Name}{Delimiter}{c.ShortName}{Delimiter}");
                    }
                }

                if (this.Speakers != null)
                {
                    foreach (var p in this.Speakers)
                    {
                        builder.Append($"{p.FullName}{Delimiter}");
                    }
                }

                this.haystack = builder.ToString();
                return this.haystack;
            }
        }

        // TODO use several speakers common photo
        public Uri SpeakerAvatar => this.Speakers.First().AvatarSmallURL;

        public string SpeakerNames => string.Join(", ", this.Speakers.Select(x => x.FullName));

        public ImageSource CommunityLogo => ImageSource.FromResource(
            this.Sessions.First().Meetup.CommunityID + ".png", typeof(RealmService).GetTypeInfo().Assembly);

        public IEnumerable<SessionModel> Sessions { get; set; }
    }
}

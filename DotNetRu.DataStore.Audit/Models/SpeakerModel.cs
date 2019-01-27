namespace DotNetRu.DataStore.Audit.Models
{
    using System.Collections.Generic;

    public sealed class SpeakerModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Biography { get; set; }

        /// <summary>
        /// Name of position such as CEO, Head of Marketing
        /// </summary>
        public string PositionName { get; set; }

        public string CompanyName { get; set; }

        public string CompanyWebsiteUrl { get; set; }

        public string BlogUrl { get; set; }

        /// <summary>
        /// Gets or sets the twitter profile: 
        /// For http://twitter.com/JamesMontemagno this is: JamesMontemagno NO @
        /// </summary>
        public string TwitterUrl { get; set; }

        /// <summary>
        /// Gets or sets the Facebook profile: 
        /// For http://facebook.com/James.Montemagno this is: James.Montemagno
        /// </summary>
        public string FacebookProfileName { get; set; }

        /// <summary>
        /// Gets or sets the linked in profile name.
        /// https://www.linkedin.com/in/jamesmontemagno we just need: jamesmontemagno
        /// </summary>
        public string LinkedInUrl { get; set; }

        /// <summary>
        /// Gets or sets the GitHub profile name.
        /// https://github.com/jamesmontemagno we just need: jamesmontemagno
        /// </summary>
        public string GitHubUrl { get; set; }

        public IEnumerable<TalkModel> Talks { get; set; }

        public string FullName => $"{this.FirstName.Trim()} {this.LastName.Trim()}";

        public string Title
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.CompanyName))
                {
                    return this.PositionName;
                }

                if (string.IsNullOrWhiteSpace(this.PositionName))
                {
                    return this.CompanyName;
                }

                return $"{this.PositionName.Trim()}, {this.CompanyName.Trim()}";
            }
        }

        public byte[] AvatarSmall { get; set; }

        public string AvatarURL { get; set; }
    }
}

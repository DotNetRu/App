namespace DotNetRu.DataStore.Audit.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using Xamarin.Forms;

    public class FriendModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string WebsiteUrl { get; set; }

        /// <summary>
        /// Gets or sets the twitter profile: 
        /// For http://twitter.com/JamesMontemagno this is: JamesMontemagno NO @
        /// </summary>
        /// <value>The twitter URL.</value>
        public string TwitterUrl { get; set; }

        /// <summary>
        /// Gets or sets the Facebook profile: 
        /// For http://facebook.com/James.Montemagno this is: James.Montemagno
        /// </summary>
        /// <value>The Facebook Profile.</value>
        public string FacebookProfileName { get; set; }

        public string LinkedInUrl { get; set; }

        public int NumberOfMeetups
        {
            get
            {
                if (this.Id == "JetBrains" || this.Id == "DotNext")
                {
                    return int.MaxValue;
                }

                return this.Meetups.Count();
            }
        }

        public IEnumerable<MeetupModel> Meetups { get; set; }

        public ImageSource LogoSmallImage { get; set; }

        public ImageSource LogoImage { get; set; }
    }
}
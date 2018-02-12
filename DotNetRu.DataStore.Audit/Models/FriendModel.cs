namespace DotNetRu.DataStore.Audit.Models
{
    using System.Linq;

    using DotNetRu.DataStore.Audit.Services;

    using Xamarin.Forms;

    using System.Collections.Generic;

    public class FriendModel : BaseModel
    {
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

                return MeetupService.GetMeetups(this.Id).Count();
            }
        }

        public IEnumerable<MeetupModel> Meetups => MeetupService.GetMeetups(this.Id).OrderBy(m => m.StartTime);

        /// <summary>
        /// Gets or sets the rank.
        /// 0 means put it at the top of the SponsorLevel
        /// </summary>
        /// <value>The rank.</value>
        public int Rank { get; set; }

        public ImageSource LogoSmallImage { get; set; }

        public ImageSource LogoImage { get; set; }
    }
}
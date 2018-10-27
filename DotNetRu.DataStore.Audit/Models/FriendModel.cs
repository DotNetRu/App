namespace DotNetRu.DataStore.Audit.Models
{
    using Xamarin.Forms;

    public class FriendModel : BaseDataObject
    {
        /// <summary>
        /// Gets or sets the name of sponsor
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }        

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Transparent PNG Rectangle
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the website URL.
        /// </summary>
        /// <value>The website URL.</value>
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

        /// <summary>
        /// Gets or sets the LinkedIn url
        /// </summary>
        /// <value>The LinkedIn Url.</value>
        public string LinkedInUrl { get; set; }

        /// <summary>
        /// Gets or sets the booth location.
        /// </summary>
        /// <value>The booth location.</value>
        public string BoothLocation { get; set; }

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
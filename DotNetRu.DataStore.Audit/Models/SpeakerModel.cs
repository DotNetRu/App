namespace DotNetRu.DataStore.Audit.Models
{
    using System;
    using System.Collections.Generic;

    using Xamarin.Forms;

    public sealed class SpeakerModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the biography.
        /// </summary>
        public string Biography { get; set; }

        /// <summary>
        /// This is the big Hero Image (Rectangle)
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// This is a small Square Image (150x150 is good)
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Name of position such as CEO, Head of Marketing
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the company website URL.
        /// </summary>
        public string CompanyWebsiteUrl { get; set; }

        /// <summary>
        /// Gets or sets the blog URL.
        /// </summary>
        public string BlogUrl { get; set; }

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
        /// Gets or sets the linked in profile name.
        /// https://www.linkedin.com/in/jamesmontemagno we just need: jamesmontemagno
        /// </summary>
        /// <value>The linked in URL.</value>
        public string LinkedInUrl { get; set; }

        public ICollection<TalkModel> Talks { get; set; }

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

        public Uri AvatarUri
        {
            get
            {
                try
                {
                    return new Uri(this.AvatarUrl);
                }
                catch
                {
                    // ignored
                }

                return null;
            }
        }

        public ImageSource AvatarImage { get; set; }

        public ImageSource PhotoImage { get; set; }

        public Uri PhotoUri
        {
            get
            {
                try
                {
                    return new Uri(this.PhotoUrl);
                }
                catch
                {
                    // ignored
                }

                return null;
            }
        }
    }
}
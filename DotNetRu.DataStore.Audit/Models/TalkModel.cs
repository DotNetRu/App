namespace DotNetRu.DataStore.Audit.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using MvvmHelpers;

    using Xamarin.Forms;

    public class TalkModel : BaseModel
    {
        private const string Delimiter = "|";

        private bool feedbackLeft;
        private string haystack;

        public TalkModel()
        {
            this.Speakers = new List<SpeakerModel>();
            this.Categories = new List<Category>();
        }

        public string TalkId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the short title that is displayed in the navigation bar
        /// For instance "Intro to X.Forms"
        /// </summary>
        /// <value>The short title.</value>
        public string ShortTitle { get; set; }

        /// <summary>
        /// Gets or sets the abstract.
        /// </summary>
        /// <value>The abstract.</value>
        public string Abstract { get; set; }

        /// <summary>
        /// Gets or sets the speakers.
        /// </summary>
        /// <value>The speakers.</value>
        public ICollection<SpeakerModel> Speakers { get; set; }

        /// <summary>
        /// Gets or sets the room.
        /// </summary>
        /// <value>The room.</value>
        public Room Room { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>The main categories.</value>
        public ICollection<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>The start time.</value>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>The end time.</value>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the level of the session [100 - 400]
        /// </summary>
        /// <value>The session level.</value>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the url to the presentation material
        /// </summary>
        public string PresentationUrl { get; set; }

        /// <summary>
        /// Gets or sets the url to the recorded session video
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        /// Gets or sets the url to the code from session
        /// </summary>
        public string CodeUrl { get; set; }

        public DateTime StartTimeOrderBy => this.StartTime ?? DateTime.MinValue;

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
                        builder.Append(
                            $"{p.FirstName} {p.LastName}{Delimiter}{p.FirstName}{Delimiter}{p.LastName}{Delimiter}");
                    }
                }

                this.haystack = builder.ToString();
                return this.haystack;
            }
        }

        public ImageSource SpeakerAvatar => this.Speakers.First().AvatarImage;

        public bool FeedbackLeft
        {
            get => this.feedbackLeft;

            set => this.SetProperty(ref this.feedbackLeft, value);
        }

        public string LevelString => $"Level: {this.Level}";
    }
}
namespace DotNetRu.DataStore.Audit.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TalkModel : BaseDataObject
    {
        public TalkModel()
        {
            this.Speakers = new List<SpeakerModel>();
            this.Categories = new List<Category>();
        }

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
        public virtual ICollection<SpeakerModel> Speakers { get; set; }

        /// <summary>
        /// Gets or sets the room.
        /// </summary>
        /// <value>The room.</value>
        public virtual Room Room { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>The main categories.</value>
        public virtual ICollection<Category> Categories { get; set; }

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
        [Newtonsoft.Json.JsonIgnore]
        public string CodeUrl { get; set; }

        private string speakerNames;

        [Newtonsoft.Json.JsonIgnore]
        public string SpeakerNames
        {
            get
            {
                if (this.speakerNames != null)
                {
                    return this.speakerNames;
                }

                this.speakerNames = string.Empty;

                if (this.Speakers == null || this.Speakers.Count == 0)
                {
                    return this.speakerNames;
                }

                var allSpeakers = this.Speakers.ToArray();
                this.speakerNames = string.Empty;
                for (int i = 0; i < allSpeakers.Length; i++)
                {
                    this.speakerNames += allSpeakers[i].FullName;
                    if (i != this.Speakers.Count - 1) this.speakerNames += ", ";
                }

                return this.speakerNames;
            }
        }

        private string speakerHandles;

        [Newtonsoft.Json.JsonIgnore]
        public string SpeakerHandles
        {
            get
            {
                if (this.speakerHandles != null)
                {
                    return this.speakerHandles;
                }

                this.speakerHandles = string.Empty;

                if (this.Speakers == null || this.Speakers.Count == 0) return this.speakerHandles;

                var allSpeakers = this.Speakers.ToArray();
                this.speakerHandles = string.Empty;
                for (int i = 0; i < allSpeakers.Length; i++)
                {
                    var handle = allSpeakers[i].TwitterUrl;
                    if (!string.IsNullOrEmpty(handle))
                    {
                        if (i != 0)
                        {
                            this.speakerHandles += ", ";
                        }

                        this.speakerHandles += $"@{handle}";
                    }
                }

                return this.speakerHandles;
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public DateTime StartTimeOrderBy => this.StartTime ?? DateTime.MinValue;

        const string delimiter = "|";

        string haystack;

        [Newtonsoft.Json.JsonIgnore]
        public string Haystack
        {
            get
            {
                if (this.haystack != null) return this.haystack;

                var builder = new StringBuilder();
                builder.Append(delimiter);
                builder.Append(this.Title);
                builder.Append(delimiter);
                if (this.Categories != null)
                {
                    foreach (var c in this.Categories) builder.Append($"{c.Name}{delimiter}{c.ShortName}{delimiter}");
                }

                if (this.Speakers != null)
                {
                    foreach (var p in this.Speakers)
                        builder.Append(
                            $"{p.FirstName} {p.LastName}{delimiter}{p.FirstName}{delimiter}{p.LastName}{delimiter}");
                }

                this.haystack = builder.ToString();
                return this.haystack;
            }
        }

        bool feedbackLeft;

        [Newtonsoft.Json.JsonIgnore]
        public bool FeedbackLeft
        {
            get => this.feedbackLeft;

            set => this.SetProperty(ref this.feedbackLeft, value);
        }

        [Newtonsoft.Json.JsonIgnore]
        public string LevelString => $"Level: {this.Level}";
    }
}
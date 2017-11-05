﻿namespace DotNetRu.DataStore.Audit.Models
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class MeetupModel : BaseDataObject
    {
        /// <summary>
        /// Gets or sets the title of the event such as: Keynote
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the descriptionof the event
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

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
        /// Gets or sets a value indicating whether this instance is all day.
        /// Such as Darwin Lounge for instance
        /// </summary>
        /// <value><c>true</c> if this instance is all day; otherwise, <c>false</c>.</value>
        public bool IsAllDay { get; set; }

        public string VenueID { get; set; }

        /// <summary>
        /// Gets or sets the sponsor if there is one for the event
        /// </summary>
        /// <value>The sponsor.</value>
        public virtual FriendModel FriendModel { get; set; }

        [JsonIgnore]
        public bool HasSponsor => this.FriendModel != null;

        [JsonIgnore]
        public DateTime StartTimeOrderBy => this.StartTime ?? DateTime.MinValue;

        [JsonIgnore]
        public IEnumerable<string> EventTalksIds { get; set; }
    }
}


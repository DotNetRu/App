﻿namespace DotNetRu.DataStore.Audit.Models
{
    /// <summary>
    /// Per user feedback
    /// </summary>
    public class Feedback : BaseDataObject
    {
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public int SessionRating { get; set; }
    }
}
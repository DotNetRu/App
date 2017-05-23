﻿using System;

namespace XamarinEvolve.DataObjects
{
    public class MiniHack : BaseDataObject
    {
        public string Name { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string GitHubUrl {get;set;}
        public string BadgeUrl { get; set; }
        public string UnlockCode { get; set; }
        public int? Score { get; set; }
        public string Category { get; set; }

        #if MOBILE
        [Newtonsoft.Json.JsonIgnore]
        public Uri BadgeUri 
        { 
            get 
            { 
                try
                {
                    return new Uri(BadgeUrl);
                }
                catch
                {

                }
                return null;
            } 
        }

        bool isCompleted;
        [Newtonsoft.Json.JsonIgnore]
        public bool IsCompleted
        {
            get { return isCompleted; }
            set { SetProperty(ref isCompleted, value); }
        }

        bool completedSynced;
        [Newtonsoft.Json.JsonIgnore]
        public bool CompletedSynced
        {
            get { return completedSynced; }
            set { SetProperty(ref completedSynced, value); }
        }
        #endif
    }
}


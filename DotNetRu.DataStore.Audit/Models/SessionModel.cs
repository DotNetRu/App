﻿namespace DotNetRu.DataStore.Audit.Models
{
    using System;

    public class SessionModel
    {
        public TalkModel Talk { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public MeetupModel Meetup { get; set; }
    }
}
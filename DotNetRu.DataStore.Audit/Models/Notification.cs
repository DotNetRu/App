﻿namespace DotNetRu.DataStore.Audit.Models
{
    using System;

    public class Notification : BaseDataObject
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}

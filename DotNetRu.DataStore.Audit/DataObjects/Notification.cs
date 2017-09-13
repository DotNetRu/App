using System;

namespace XamarinEvolve.DataObjects
{
    using DotNetRu.DataStore.Audit.DataObjects;

    public class Notification : BaseDataObject
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}


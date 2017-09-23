namespace DotNetRu.DataStore.Audit.DataObjects
{
    using System;
    using MvvmHelpers;

    public interface IBaseDataObject
    {
        string Id { get; set; }
    }

    public class BaseDataObject : ObservableObject, IBaseDataObject
    {
        public BaseDataObject()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string RemoteId { get; set; }

        [Newtonsoft.Json.JsonProperty("Id")]
        public string Id { get; set; }
    }
}


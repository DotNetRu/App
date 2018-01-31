namespace DotNetRu.DataStore.Audit.Models
{
    public class ConferenceFeedback : BaseModel
    {
        public string UserId { get; set; }

        public int Question1 { get; set; }

        public string DeviceOS { get; set; }

        public string AppVersion { get; set; }
    }
}

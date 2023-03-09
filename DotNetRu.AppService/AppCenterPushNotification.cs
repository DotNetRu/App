using Newtonsoft.Json;

namespace DotNetRu.Azure
{
    public class AppCenterPushNotification
    {
        [JsonProperty("notification_content")]
        public NotificationContent NotificationContent { get; set; }

        [JsonProperty("notification_target")]
        public object NotificationTarget { get; set; }
    }

    public class NotificationContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}

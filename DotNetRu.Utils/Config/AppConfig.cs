using System.Text;
using DotNetRu.Utils.Helpers;
using Newtonsoft.Json;

namespace DotNetRu.Clients.UI
{
    public class AppConfig
    {
        public string AppCenterAndroidKey { get; set; }

        public string AppCenteriOSKey { get; set; }

        public string PushNotificationsChannel { get; set; }

        public static AppConfig GetConfig()
        {
            var configBytes = ResourceHelper.ExtractResource("DotNetRu.Clients.UI.config.json");
            var configBytesAsString = Encoding.UTF8.GetString(configBytes);
            return JsonConvert.DeserializeObject<AppConfig>(configBytesAsString);
        }
    }
}

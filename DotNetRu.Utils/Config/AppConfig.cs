using System.Text;
using DotNetRu.Utils.Helpers;
using Newtonsoft.Json;

namespace DotNetRu.Clients.UI
{
    public class AppConfig
    {
        public string AppCenterAndroidKey { get; set; }

        public string AppCenteriOSKey { get; set; }

        public string TweetFunctionUrl { get; set; }

        public string RealmDatabase { get; set; }

        public string RealmServerUrl { get; set; }

        public static AppConfig GetConfig()
        {
            var configBytes = ResourceHelper.ExtractResource("DotNetRu.Utils.Config.config.json");
            var configBytesAsString = Encoding.UTF8.GetString(configBytes);
            return JsonConvert.DeserializeObject<AppConfig>(configBytesAsString);
        }
    }
}

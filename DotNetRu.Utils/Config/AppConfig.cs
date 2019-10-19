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
#if DEBUG 
            return new AppConfig()
            {
                AppCenterAndroidKey = "6f9a7703-8ca4-477e-9558-7e095f7d20aa",
                AppCenteriOSKey = "1e7f311f-1055-4ec9-8b00-0302015ab8ae",
                TweetFunctionUrl = "https://dotnettweetservice.azurewebsites.net/api/Tweets",
                RealmDatabase = "dotnetru_050919",
                RealmServerUrl = "dotnetru.de1a.cloud.realm.io"
        };
#endif

#pragma warning disable CS0162 // Unreachable code detected
            var configBytes = ResourceHelper.ExtractResource("DotNetRu.Utils.Config.config.json");
            var configBytesAsString = Encoding.UTF8.GetString(configBytes);
            return JsonConvert.DeserializeObject<AppConfig>(configBytesAsString);
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}

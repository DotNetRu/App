using System.Net.Http;
using System.Threading.Tasks;
using DotNetRu.Azure;

namespace DotNetRu.AzureService
{
    public class PushNotificationsManager
    {
        private readonly PushSettings pushSettings;

        public PushNotificationsManager(PushSettings pushSettings)
        {
            this.pushSettings = pushSettings;
        }

        public async Task SendPushNotifications(PushContent pushContent)
        {
            var appCenterPushUrl = "https://api.appcenter.ms/v0.1/apps/DotNetRu/{0}/push/notifications";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-API-Token", pushSettings.AppCenterApiToken);

            var appCenterPushNotification = new AppCenterPushNotification
            {
                NotificationContent = new NotificationContent()
                {
                    Name = "Conference update",
                    Body = pushContent.Body,
                    Title = pushContent.Title
                },
                NotificationTarget = null
            };

            foreach (var app in pushSettings.AppCenterAppNames)
            {
                var push = await httpClient.PostAsJsonAsync(string.Format(appCenterPushUrl, app), appCenterPushNotification);
                push.EnsureSuccessStatusCode();
            }
        }
    }
}

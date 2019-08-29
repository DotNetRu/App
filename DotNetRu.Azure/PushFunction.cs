using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace DotNetRu.Azure
{
    public static class PushFunction
    {
        [FunctionName("Push")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "{appType}/push")] HttpRequest request,
            string appType,
            ILogger logger,
            ExecutionContext context)
        {
            try
            {
                var appCenterPushUrl = "https://api.appcenter.ms/v0.1/apps/DotNetRu/{0}/push/notifications";

                var json = await request.ReadAsStringAsync();
                var pushContent = JsonConvert.DeserializeObject<PushContent>(json);

                logger.LogInformation("Function is called with {AppType}, {Title}, {Body}", appType, pushContent.Title, pushContent.Body);

                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("pushconfig.json", optional: true, reloadOnChange: true)
                    .Build();

                var pushConfigs = config
                    .GetSection("PushConfig")
                    .Get<PushConfig[]>();

                var pushConfig = pushConfigs.Single(x => x.AppType == appType);

                var httpClient = new HttpClient();

                var apiHeader = request.Headers["X-API-Token"].ToString();
                var apiToken = pushConfig.ApiToken;

                if (apiHeader != apiToken)
                {
                    return new UnauthorizedResult();
                }

                httpClient.DefaultRequestHeaders.Add("X-API-Token", pushConfig.AppCenterApiToken);

                var appCenterPushNotification = new AppCenterPushNotification()
                {
                    NotificationContent = new NotificationContent()
                    {
                        Name = "Conference update",
                        Body = pushContent.Body,
                        Title = pushContent.Title
                    },
                    NotificationTarget = null
                };

                foreach (var app in pushConfig.AppCenterAppNames)
                {
                    var push = await httpClient.PostAsJsonAsync(string.Format(appCenterPushUrl, app), appCenterPushNotification);
                    push.EnsureSuccessStatusCode();
                }
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Error while sending push notifications");
                return new ObjectResult(e.Message) {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return new OkResult();
        }
    }
}

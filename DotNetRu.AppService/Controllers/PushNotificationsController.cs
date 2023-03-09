using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using DotNetRu.AzureService;

namespace DotNetRu.Azure
{
    [Route("push-notifications")]
    public class PushNotificationsController
    {
        private readonly ILogger logger;

        private readonly PushNotificationsManager pushNotificationsManager;

        public PushNotificationsController(
            ILogger<PushNotificationsController> logger,
            PushNotificationsManager pushNotificationsManager)
        {
            this.logger = logger;
            this.pushNotificationsManager = pushNotificationsManager;
        }

        [HttpPost]
        public async Task<IActionResult> Run([FromBody] PushContent pushContent)
        {
            try
            {
                await pushNotificationsManager.SendPushNotifications(pushContent);
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Error while sending push notifications");
                return new ObjectResult(e.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return new OkResult();
        }


    }
}

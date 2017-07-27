using System.Net;
using System.Net.Http;
using System.Web.Http;
using XamarinEvolve.Backend.Models;
using System.Threading.Tasks;
using Microsoft.Azure.Mobile.Server.Config;
using System.Configuration;

namespace XamarinEvolve.Backend.Controllers
{
    [MobileAppController]
    public class EvolveNotificationsController : ApiController
    {        
        public async Task<HttpResponseMessage> Post(string pns, string password, [FromBody]string message)
        {
            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;

            if (string.IsNullOrWhiteSpace(message) || password != ConfigurationManager.AppSettings["NotificationsPassword"])
                return Request.CreateResponse(HttpStatusCode.Forbidden);

            var ret = HttpStatusCode.InternalServerError;

            try
            {
                switch (pns.ToLower())
                {
                    case "wns":
                        // Windows 8.1 / Windows Phone 8.1
                        var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" +
                                    message + "</text></binding></visual></toast>";
                        outcome = await EvolveNotifications.Instance.Hub.SendWindowsNativeNotificationAsync(toast);
                        break;
                    case "apns":
                        // iOS
                        var alert = "{\"aps\":{\"alert\":\"" + message + "\"}}";
                        outcome = await EvolveNotifications.Instance.Hub.SendAppleNativeNotificationAsync(alert);
                        break;
                    case "gcm":
                        // Android
                        var notif = "{ \"data\" : {\"message\":\"" + message + "\"}}";
                        outcome = await EvolveNotifications.Instance.Hub.SendGcmNativeNotificationAsync(notif);
                        break;
                }

                if (outcome != null)
                {
                    if (!((outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                        (outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown)))
                    {
                        ret = HttpStatusCode.OK;
                    }
                }
            }
            catch
            {
                ret = HttpStatusCode.InternalServerError;
            }

            return Request.CreateResponse(ret);
        }
    }
}

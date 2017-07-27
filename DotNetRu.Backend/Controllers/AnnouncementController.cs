using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using XamarinEvolve.DataObjects;
using System;
using XamarinEvolve.Backend.Models;
using System.Configuration;

namespace XamarinEvolve.Backend.Controllers
{
    [MobileAppController]
    public class AnnouncementController : ApiController
    {
        // POST api/Announcement

        public async Task<HttpResponseMessage> Post(string password, [FromBody]string message)
        {
            if (string.IsNullOrWhiteSpace(message) || password != ConfigurationManager.AppSettings["NotificationsPassword"])
                return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                var accounenement = new Notification
                {
                    Date = DateTime.UtcNow,
                    Text = message
                };

                var context = new XamarinEvolveContext();

                context.Notifications.Add(accounenement);

                await context.SaveChangesAsync();

            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}

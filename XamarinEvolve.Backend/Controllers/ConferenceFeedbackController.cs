using Microsoft.Azure.Mobile.Server;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using XamarinEvolve.Backend.Helpers;
using XamarinEvolve.Backend.Models;
using XamarinEvolve.DataObjects;

namespace XamarinEvolve.Backend.Controllers
{
    public class ConferenceFeedbackController : TableController<ConferenceFeedback>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            XamarinEvolveContext context = new XamarinEvolveContext();
            DomainManager = new EntityDomainManager<ConferenceFeedback>(context, Request, true);
        }

        [Authorize]
        public IQueryable<ConferenceFeedback> GetAllConferenceFeedback()
        {
            var items = Query();

            var userId = AuthenticationHelper.GetAuthenticatedUserId(RequestContext);

            var final = items.Where(feedback => feedback.UserId == userId);

            return final;
        }

        [Authorize]
        public SingleResult<ConferenceFeedback> GetFeedback(string id)
        {
            return Lookup(id);
        }

        [Authorize]
        public Task<ConferenceFeedback> PatchFeedback(string id, Delta<ConferenceFeedback> patch)
        {
            return UpdateAsync(id, patch);
        }

        [Authorize]
        public async Task<IHttpActionResult> PostFeedback(ConferenceFeedback item)
        {
            var feedback = item;

            feedback.UserId = AuthenticationHelper.GetAuthenticatedUserId(RequestContext);
            var current = await InsertAsync(feedback);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }
    }
}

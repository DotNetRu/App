using Microsoft.Azure.Mobile.Server.Config;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using XamarinEvolve.Backend.Helpers;
using XamarinEvolve.Backend.Models;

namespace XamarinEvolve.Backend.Controllers
{
    [MobileAppController]
    public class CompleteMiniHackController: ApiController
    {
        [Authorize]
        public async Task<IHttpActionResult> Post([FromBody] string id)
        {
            var userId = AuthenticationHelper.GetAuthenticatedUserId(RequestContext);

            using (var context = new XamarinEvolveContext())
            {
                var getCountQuery = @"SELECT COUNT(1) FROM [dbo].[MiniHackCompletions] WHERE UserId = {0} AND HackId = {1}";

                var count = await context.Database.SqlQuery<int>(getCountQuery, userId, id).SingleAsync();

                if (count == 0)
                {
                    var insertQuery = @"INSERT [dbo].[MiniHackCompletions] (UserId, HackId) VALUES ({0}, {1})";
                    context.Database.ExecuteSqlCommand(insertQuery, userId, id);
                }
            }

            return Ok();
        }

        public async Task<int> Get(string id)
        {
            int count = 0;

            using (var context = new XamarinEvolveContext())
            {
                var getCountQuery = @"SELECT COUNT(1) FROM [dbo].[MiniHackCompletions] WHERE HackId = {0}";

                count = await context.Database.SqlQuery<int>(getCountQuery, id).SingleAsync();
            }

            return count;
        }
    }
}
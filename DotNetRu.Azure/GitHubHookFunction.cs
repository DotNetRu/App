using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace DotNetRu.Azure
{
    public static class GitHubHookFunction
    {
        [FunctionName("update")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger logger)
        {
            var httpClient = new HttpClient();

            var realmNameHeaderValue = req.Headers["RealmName"].ToString();

            if (!string.IsNullOrEmpty(realmNameHeaderValue))
            {
                httpClient.DefaultRequestHeaders.Add("RealmName", realmNameHeaderValue);
            }

            httpClient.PostAsync("https://dotnetruapp.azurewebsites.net/api/realmUpdate", new StringContent(""));

            return new OkResult();
        }
    }
}

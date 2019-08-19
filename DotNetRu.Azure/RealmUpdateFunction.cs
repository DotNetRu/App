using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Realms.Sync;
using Realms;
using System;
using System.IO;
using DotNetRu.RealmUpdate;
using Microsoft.Extensions.Configuration;

namespace DotNetRu.Azure
{
    public static class RealmUpdateFunction
    {
        public static string CurrentRealmName = "dotnet_prod_090819";

        [FunctionName("update")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var realmName = req.Headers.ContainsKey("RealmName")
                    ? req.Headers["RealmName"].ToString()
                    : CurrentRealmName;

                log.LogInformation("Realm to update {RealmName}", realmName);

                var realmUrl = new Uri($"realms://dotnet.de1a.cloud.realm.io/{realmName}");

                var realmData = await UpdateManager.GetAuditData();

                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                    .Build();

                var user = await User.LoginAsync(
                    Credentials.UsernamePassword(config["Login"], config["Password"], createUser: false), 
                    new Uri("https://dotnet.de1a.cloud.realm.io"));

                var tempRealmFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                var syncConfiguration = new FullSyncConfiguration(realmUrl, user, tempRealmFile);

                var realm = await Realm.GetInstanceAsync(syncConfiguration);

                UpdateManager.UpdateRealm(realm, realmData);
            }
            catch (Exception e)
            {
                log.LogCritical(e, "Error while updating realm");
                return new ObjectResult(e.Message) { StatusCode = StatusCodes.Status500InternalServerError };
            }

            return new OkResult();
        }
    }
}

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
using DotNetRu.DataStore.Audit.RealmModels;
using System.Linq;
using System.Net.Http;

namespace DotNetRu.Azure
{
    public static class RealmUpdateFunction
    {
        public static string CurrentRealmName = "dotnetru_prod_090819";

        [FunctionName("realmUpdate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger logger,
            ExecutionContext context)
        {
            try
            {
                var realmName = req.Headers.ContainsKey("RealmName")
                    ? req.Headers["RealmName"].ToString()
                    : CurrentRealmName;

                logger.LogInformation("Realm to update {RealmName}", realmName);

                var realmUrl = new Uri($"realms://dotnet.de1a.cloud.realm.io/{realmName}");

                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                    .Build();

                var user = await GetUser(logger, config);

                var realm = await GetRealm(realmUrl, user);

                var currentVersion = GetCurrentVersion(realm);

                var updateDelta = await UpdateManager.GetAuditUpdate(currentVersion, logger);

                realm = await GetRealm(realmUrl, user);
                RealmHelper.UpdateRealm(realm, updateDelta);

                // TODO separate DBs by different Realm Clouds
                var environment = realmName.Contains("prod") ? "production" : "beta";

                var pushConfigs = ConfigManager.GetPushConfigs(context);
                var pushConfig = pushConfigs.Single(c => c.AppType == environment);

                foreach (var meetup in updateDelta.Meetups.Where(meetup => meetup.Sessions.First().StartTime > DateTime.Now))
                {
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Add("X-API-Token", pushConfig.ApiToken);

                    var pushContent = new PushContent()
                    {
                        Title = $"{meetup.Name} is announced!",
                        Body = "Open DotNetRu app for details"
                    };

                    await httpClient.PostAsJsonAsync($"https://dotnetruapp.azurewebsites.net/api/{environment}/push", pushContent);
                }
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Error while updating realm");
                return new ObjectResult(e) { StatusCode = StatusCodes.Status500InternalServerError };
            }

            return new OkResult();
        }

        private static async Task<Realm> GetRealm(Uri realmUrl, User user)
        {
            var tempRealmFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var syncConfiguration = new FullSyncConfiguration(realmUrl, user, tempRealmFile);

            return await Realm.GetInstanceAsync(syncConfiguration);
        }

        private static string GetCurrentVersion(Realm realm)
        {
            var auditVersion = realm.All<AuditVersion>();

            return auditVersion.Single().CommitHash;
        }

        private static async Task<User> GetUser(ILogger log, IConfigurationRoot config)
        {
            User user;
            try
            {
                user = await User.LoginAsync(
                    Credentials.UsernamePassword(config["Login"], config["Password"], createUser: false),
                    new Uri("https://dotnet.de1a.cloud.realm.io"));
            }
            catch (Exception e)
            {
                log.LogCritical(e, "Error while logging in");
                user = await User.LoginAsync(
                    Credentials.UsernamePassword(config["Login"], config["Password"], createUser: false),
                    new Uri("https://dotnet.de1a.cloud.realm.io"));
            }

            return user;
        }
    }
}

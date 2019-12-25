using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using Realms.Sync;
using Realms;
using DotNetRu.DataStore.Audit.RealmModels;
using DotNetRu.RealmUpdateLibrary;
using DotNetRu.AzureService;

namespace DotNetRu.Azure
{
    [Route("realm-update")]
    public class RealmUpdateController : ControllerBase
    {
        private readonly ILogger logger;

        private readonly AppSettings appSettings;

        public RealmUpdateController(
            ILogger<RealmUpdateController> logger,
            AppSettings appSettings)
        {
            this.logger = logger;
            this.appSettings = appSettings;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMobileData()
        {
            try
            {
                logger.LogInformation("Realm to update {RealmServerUrl}/{RealmName}", appSettings.RealmServerUrl, appSettings.RealmName);

                var user = await this.GetUser();

                var realmUrl = new Uri($"realms://{appSettings.RealmServerUrl}/{appSettings.RealmName}");
                var realm = await GetRealm(realmUrl, user);

                var currentVersion = GetCurrentVersion(realm);
                var updateDelta = await UpdateManager.GetAuditUpdate(currentVersion, logger);

                realm = await GetRealm(realmUrl, user);
                RealmHelper.UpdateRealm(realm, updateDelta);

                return new OkObjectResult(appSettings);

                //var environment = realmServerUrl.Contains("dotnetru") ? "beta" : "production";

                //var pushConfigs = ConfigManager.GetPushConfigs(context);
                //var pushConfig = pushConfigs.Single(c => c.AppType == environment);

                //foreach (var meetup in updateDelta.Meetups.Where(meetup => meetup.Sessions.First().StartTime > DateTime.Now))
                //{
                //    var httpClient = new HttpClient();
                //    httpClient.DefaultRequestHeaders.Add("X-API-Token", pushConfig.ApiToken);

                //    var pushContent = new PushContent()
                //    {
                //        Title = $"{meetup.Name} is announced!",
                //        Body = "Open DotNetRu app for details"
                //    };

                //    await httpClient.PostAsJsonAsync($"https://dotnetruapp.azurewebsites.net/api/{environment}/push", pushContent);
                //}
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Unhanled error while updating realm");
                return new ObjectResult(e) { 
                    StatusCode = StatusCodes.Status500InternalServerError 
                };
            }
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

        private async Task<User> GetUser()
        {
            return await Realms.Sync.User.LoginAsync(
                    Credentials.UsernamePassword(appSettings.Login, appSettings.Password, createUser: false),
                    new Uri($"https://{appSettings.RealmServerUrl}"));
        }
    }
}

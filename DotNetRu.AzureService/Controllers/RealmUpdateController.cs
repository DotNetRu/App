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

        private readonly RealmSettings realmSettings;

        private readonly PushNotificationsManager pushNotificationsManager;

        public RealmUpdateController(
            ILogger<RealmUpdateController> logger,
            RealmSettings appSettings,
            PushNotificationsManager pushNotificationsManager)
        {
            this.logger = logger;
            this.realmSettings = appSettings;
            this.pushNotificationsManager = pushNotificationsManager;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMobileData()
        {
            try
            {
                logger.LogInformation("Realm to update {RealmServerUrl}/{RealmName}", realmSettings.RealmServerUrl, realmSettings.RealmName);

                var user = await this.GetUser();

                var realmUrl = new Uri($"realms://{realmSettings.RealmServerUrl}/{realmSettings.RealmName}");
                var realm = await GetRealm(realmUrl, user);

                var currentVersion = GetCurrentVersion(realm);
                var updateDelta = await UpdateManager.GetAuditUpdate(currentVersion, logger);

                realm = await GetRealm(realmUrl, user);
                RealmHelper.UpdateRealm(realm, updateDelta);

                foreach (var meetup in updateDelta.Meetups.Where(meetup => meetup.Sessions.First().StartTime > DateTime.Now))
                {
                    var pushContent = new PushContent()
                    {
                        Title = $"{meetup.Name} is announced!",
                        Body = "Open DotNetRu app for details"
                    };


                    await pushNotificationsManager.SendPushNotifications(pushContent);
                }

                return new OkObjectResult(realmSettings);
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Unhandled error while updating realm");
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
                    Credentials.UsernamePassword(realmSettings.Login, realmSettings.Password, createUser: false),
                    new Uri($"https://{realmSettings.RealmServerUrl}"));
        }
    }
}

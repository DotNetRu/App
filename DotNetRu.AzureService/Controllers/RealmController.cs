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
using Microsoft.AspNetCore.Hosting;

namespace DotNetRu.Azure
{
    [Route("realm")]
    public class RealmController : ControllerBase
    {
        private readonly ILogger logger;

        private readonly RealmSettings realmSettings;

        private readonly PushNotificationsManager pushNotificationsManager;

        private readonly IWebHostEnvironment webHostEnvironment;

        public RealmController(
            ILogger<DiagnosticsController> logger,
            RealmSettings appSettings,
            PushNotificationsManager pushNotificationsManager,
            IWebHostEnvironment webHostEnvironment)
        {
            this.logger = logger;
            this.realmSettings = appSettings;
            this.pushNotificationsManager = pushNotificationsManager;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateMobileData()
        {
            try
            {
                logger.LogInformation("Realm to update {RealmServerUrl}/{RealmName}", realmSettings.RealmServerUrl, realmSettings.RealmName);

                var user = await this.GetUser();

                var realmUrl = new Uri($"realms://{realmSettings.RealmServerUrl}/{realmSettings.RealmName}");
                var realm = await GetRealm(realmUrl, user);

                var currentVersion = GetCurrentVersion(realm);
                var auditUpdate = await UpdateManager.GetAuditUpdate(currentVersion, logger);

                realm = await GetRealm(realmUrl, user);
                DotNetRuRealmHelper.UpdateRealm(realm, auditUpdate);

                foreach (var meetup in auditUpdate.Meetups.Where(meetup => meetup.Sessions.First().StartTime > DateTime.Now))
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

        [HttpGet]
        [Route("generate/offline")]
        public async Task<FileStreamResult> GenerateOfflineRealm([FromQuery] string commitSha)
        {
            var auditData = commitSha != null
                ? await UpdateManager.GetAuditData(commitSha)
                : await UpdateManager.GetAuditData();

            var realmOfflinePath = $"{Path.GetTempPath()}/DotNetRuOffline.realm";
            Realm.DeleteRealm(new RealmConfiguration(realmOfflinePath));

            var realm = Realm.GetInstance(realmOfflinePath);
            DotNetRuRealmHelper.ReplaceRealm(realm, auditData);

            var stream = System.IO.File.OpenRead(realmOfflinePath);
            return new FileStreamResult(stream, "application/octet-stream")
            {
                FileDownloadName = "DotNetRuOffline.realm"
            };
        }

        [HttpGet]
        [Route("generate/online")]
        public async Task<IActionResult> GenerateOnlineRealm(
            [FromQuery] string commitSha, 
            [FromQuery] string realmServerUrl, 
            [FromQuery] string realmName)
        {
            var auditData = commitSha != null
                ? await UpdateManager.GetAuditData(commitSha)
                : await UpdateManager.GetAuditData();

            logger.LogInformation("Realm to update {RealmServerUrl}/{RealmName}", realmServerUrl, realmName);

            var user = await this.GetUser();

            var realmUrl = new Uri($"realms://{realmServerUrl}/{realmName}");
            var realm = await GetRealm(realmUrl, user);

            realm = await GetRealm(realmUrl, user);
            DotNetRuRealmHelper.ReplaceRealm(realm, auditData);

            return new OkObjectResult("Success");
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
            SyncConfigurationBase.Initialize(UserPersistenceMode.NotEncrypted, basePath: Path.GetTempPath());

            return await Realms.Sync.User.LoginAsync(
                    Credentials.UsernamePassword(realmSettings.Login, realmSettings.Password, createUser: false),
                    new Uri($"https://{realmSettings.RealmServerUrl}"));
        }
    }
}

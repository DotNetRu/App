using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using Realms;
using DotNetRu.RealmUpdateLibrary;
using DotNetRu.AzureService;
using DotNetRu.AzureService.Helpers;

namespace DotNetRu.Azure
{
    [Route("realm")]
    public class RealmController : ControllerBase
    {
        private readonly ILogger logger;

        private readonly RealmSettings realmSettings;

        private readonly PushNotificationsManager pushNotificationsManager;

        public RealmController(
            ILogger<DiagnosticsController> logger,
            RealmSettings appSettings,
            PushNotificationsManager pushNotificationsManager)
        {
            this.logger = logger;
            this.realmSettings = appSettings;
            this.pushNotificationsManager = pushNotificationsManager;
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateMobileData()
        {
            try
            {
                logger.LogInformation("Realm to update {RealmServerUrl}/{RealmName}", realmSettings.RealmServerUrl, realmSettings.RealmName);

                var user = await realmSettings.GetUser();

                var realmUrl = new Uri($"realms://{realmSettings.RealmServerUrl}/{realmSettings.RealmName}");
                var realm = await user.GetRealm(realmUrl);

                var currentVersion = realm.GetCurrentVersion();
                var auditUpdate = await UpdateManager.GetAuditUpdate(currentVersion, logger);

                realm = await user.GetRealm(realmUrl);
                DotNetRuRealmHelper.UpdateRealm(realm, auditUpdate);

                await SendMeetupsNotifications(auditUpdate);

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

        [HttpPost]
        [Route("trigger_update")]
        public async Task<IActionResult> UpdateMobileDataAsync()
        {
            logger.LogInformation("Realm to update {RealmServerUrl}/{RealmName}", realmSettings.RealmServerUrl, realmSettings.RealmName);

            UpdateOnlineRealm();

            return new OkObjectResult(realmSettings);
        }

        private async Task UpdateOnlineRealm()
        {
            var user = await realmSettings.GetUser();

            var realmUrl = new Uri($"realms://{realmSettings.RealmServerUrl}/{realmSettings.RealmName}");
            var realm = await user.GetRealm(realmUrl);

            var currentVersion = realm.GetCurrentVersion();
            var auditUpdate = await UpdateManager.GetAuditUpdate(currentVersion, logger);

            DotNetRuRealmHelper.UpdateRealm(realm, auditUpdate);

            await SendMeetupsNotifications(auditUpdate);
        }

        private async Task SendMeetupsNotifications(AuditXmlUpdate auditUpdate)
        {
            foreach (var meetup in auditUpdate.Meetups.Where(meetup => meetup.Sessions.First().StartTime > DateTime.Now))
            {
                var pushContent = new PushContent()
                {
                    Title = $"{meetup.Name} is announced!",
                    Body = "Open DotNetRu app for details"
                };

                await pushNotificationsManager.SendPushNotifications(pushContent);
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

            var user = await realmSettings.GetUser();

            var realmUrl = new Uri($"realms://{realmServerUrl}/{realmName}");
            var realm = await user.GetRealm(realmUrl);
            DotNetRuRealmHelper.ReplaceRealm(realm, auditData);

            return new OkObjectResult("Success");
        }
    }
}

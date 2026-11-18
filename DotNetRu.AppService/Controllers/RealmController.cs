using System.IO;
using System.Threading.Tasks;
using DotNetRu.RealmUpdateLibrary;
using Microsoft.AspNetCore.Mvc;
using Realms;

namespace DotNetRu.Azure
{
    [Route("realm")]
    public class RealmController : ControllerBase
    {
        private readonly UpdateManager updateManager;

        public RealmController(UpdateManager updateManager)
        {
            this.updateManager = updateManager;
        }

        [HttpGet]
        [Route("generate/offline")]
        public async Task<FileStreamResult> GenerateOfflineRealm([FromQuery] string commitSha)
        {
            var auditData = commitSha != null
                ? await this.updateManager.GetAuditXmlData(commitSha)
                : await this.updateManager.GetAuditXmlData();

            var realmOfflinePath = $"{Path.GetTempPath()}/DotNetRuOffline.realm";
            Realm.DeleteRealm(new RealmConfiguration(realmOfflinePath));

            using var realm = Realm.GetInstance(realmOfflinePath);
            DotNetRuRealmHelper.ReplaceRealm(realm, auditData);
            realm.Dispose();

            var stream = System.IO.File.OpenRead(realmOfflinePath);
            return new FileStreamResult(stream, "application/octet-stream")
            {
                FileDownloadName = "DotNetRuOffline.realm"
            };
        }
    }
}

namespace DotNetRu.DataStore.Audit.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Xml.Serialization;

    using AutoMapper;

    using DotNetRu.DataStore.Audit.Extensions;
    using DotNetRu.DataStore.Audit.RealmModels;
    using DotNetRu.DataStore.Audit.XmlEntities;

    using Octokit;

    using Realms;

    public static class UpdateService
    {
        private const int DotNetRuAppRepositoryID = 89862917;

        public static void UpdateAudit()
        {
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("DotNetRu"));

                var contentUpdate = client.Repository.Commit.Compare(
                    DotNetRuAppRepositoryID,
                    "3ddd7e73f395c0e5214aefddc912d9ac45689925",
                    "master").Result;

                var xmlFiles = contentUpdate.Files.Where(x => x.Filename.EndsWith(".xml")).ToArray();

                using (var trans = RealmService.AuditRealm.BeginWrite())
                {
                    UpdateModels<SpeakerEntity>(xmlFiles, "speakers");
                    UpdateModels<FriendEntity>(xmlFiles, "friends");
                    UpdateModels<VenueEntity>(xmlFiles, "venues");
                    UpdateModels<TalkEntity>(xmlFiles, "talks");
                    UpdateModels<MeetupEntity>(xmlFiles, "meetups");

                    var speakerPhotos = contentUpdate.Files.Where(x => x.Filename.EndsWith("avatar.jpg"));
                    UpdateSpeakerAvatars(speakerPhotos);

                    trans.Commit();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private static void UpdateSpeakerAvatars(IEnumerable<GitHubCommitFile> speakerPhotos)
        {
            Uri rootUri = new Uri("https://raw.githubusercontent.com/DotNetRu/Audit/master/db/");

            foreach (GitHubCommitFile gitHubCommitFile in speakerPhotos)
            {
                var speakerID = gitHubCommitFile.Filename.Split('/')[2];

                var dataUri = rootUri.Append("speakers", speakerID, "avatar.jpg");
                byte[] speakerAvatar = new HttpClient().GetByteArrayAsync(dataUri).Result;

                var speaker = RealmService.AuditRealm.Find<Speaker>(speakerID);
                speaker.Avatar = speakerAvatar;
            }
        }

        private static void UpdateModels<T>(IEnumerable<GitHubCommitFile> xmlFiles, string entityName)
        {
            var newEntities = xmlFiles.Where(x => x.Filename.Contains(entityName));
            UpdateModels<T>(newEntities);
        }

        private static void UpdateModels<T>(IEnumerable<GitHubCommitFile> files)
        {
            foreach (GitHubCommitFile file in files)
            {
                string fileContent = DownloadFileContent(file);

                using (var reader = new StringReader(fileContent))
                {
                    try
                    {
                        var m = new XmlSerializer(typeof(T)).Deserialize(reader);

                        var realmType = Mapper.Configuration.GetAllTypeMaps().Single(x => x.SourceType == typeof(T))
                            .DestinationType;
                        var realmObject = Mapper.Map(m, typeof(T), realmType);

                        RealmService.AuditRealm.Add(realmObject as RealmObject, update: true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        private static string DownloadFileContent(GitHubCommitFile file)
        {
            var httpClient = new HttpClient();
            return httpClient.GetStringAsync(file.RawUrl).Result;
        }
    }
}
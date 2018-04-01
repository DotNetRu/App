namespace DotNetRu.DataStore.Audit.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using AutoMapper;

    using DotNetRu.DataStore.Audit.XmlEntities;

    using Octokit;

    using Realms;

    public static class UpdateService
    {
        private const int DotNetRuAppRepositoryID = 89862917;

        private static readonly string ByteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

        public static void UpdateAudit()
        {
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("DotNetRu"));
                var tokenAuth = new Credentials("beddf618094e027cf268398b5698746f64db115f");
                client.Credentials = tokenAuth;

                var contentUpdate = client.Repository.Commit.Compare(DotNetRuAppRepositoryID, "3ddd7e73f395c0e5214aefddc912d9ac45689925", "master").Result;

                var xmlFiles = contentUpdate.Files.Where(x => x.Filename.EndsWith(".xml")).ToArray();

                UpdateModels<SpeakerEntity>(xmlFiles, "speakers");
                UpdateModels<FriendEntity>(xmlFiles, "friends");
                UpdateModels<VenueEntity>(xmlFiles, "venues");
                UpdateModels<TalkEntity>(xmlFiles, "talks");
                UpdateModels<MeetupEntity>(xmlFiles, "meetups");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void UpdateModels<T>(IEnumerable<GitHubCommitFile> xmlFiles, string entityName)
        {
            var newSpeakers = xmlFiles.Where(x => x.Filename.Contains(entityName));
            UpdateModels<T>(newSpeakers);
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

                        var realmType = Mapper.Configuration.GetAllTypeMaps().Single(x => x.SourceType == typeof(T)).DestinationType;
                        var realmObject = Mapper.Map(m, typeof(T), realmType);

                        RealmService.Put(realmObject as RealmObject);
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
            var client = new GitHubClient(new ProductHeaderValue("DotNetRu"));
            var tokenAuth = new Credentials("beddf618094e027cf268398b5698746f64db115f");
            client.Credentials = tokenAuth;

            var fileContent = client.Repository.Content.GetAllContents(DotNetRuAppRepositoryID, file.Filename).Result.FirstOrDefault().Content;
            if (fileContent.StartsWith(ByteOrderMarkUtf8))
            {
                fileContent = fileContent.Remove(0, ByteOrderMarkUtf8.Length);
            }

            return fileContent;
        }
    }
}
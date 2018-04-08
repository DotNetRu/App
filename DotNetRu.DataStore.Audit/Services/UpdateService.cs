namespace DotNetRu.DataStore.Audit.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    using AutoMapper;

    using DotNetRu.DataStore.Audit.RealmModels;
    using DotNetRu.DataStore.Audit.XmlEntities;
    using DotNetRu.Utils;

    using Octokit;

    using Realms;

    public static class UpdateService
    {
        private const int DotNetRuAppRepositoryID = 89862917;

        public static async Task UpdateAudit()
        {
            try
            {
                Console.WriteLine("AuditUpdate. Started updating audit");

                var client = new GitHubClient(new ProductHeaderValue("DotNetRu"));

                var contentUpdate = await client.Repository.Commit.Compare(
                                        DotNetRuAppRepositoryID,
                                        "332cff30d041aaf991579f10b5578206e1f28601",
                                        "master");

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                var streamTasks = contentUpdate.Files.Select(
                    async file => new UpdatedFile
                    {
                        Filename = file.Filename,
                        Content = await new HttpClient().GetByteArrayAsync(file.RawUrl).ConfigureAwait(false)
                    });
                var fileContents = await Task.WhenAll(streamTasks);

                Console.WriteLine("AuditUpdate. Downloading files time: " + stopwatch.Elapsed.ToString("g"));

                var xmlFiles = fileContents.Where(x => x.Filename.EndsWith(".xml")).ToList();

                using (var trans = RealmService.AuditRealm.BeginWrite())
                {
                    RealmService.InitializeAutoMapper();

                    UpdateModels<SpeakerEntity>(xmlFiles, "speakers");
                    UpdateModels<FriendEntity>(xmlFiles, "friends");
                    UpdateModels<VenueEntity>(xmlFiles, "venues");
                    UpdateModels<TalkEntity>(xmlFiles, "talks");
                    UpdateModels<MeetupEntity>(xmlFiles, "meetups");

                    var speakerPhotos = fileContents.Where(x => x.Filename.EndsWith("avatar.jpg"));
                    UpdateSpeakerAvatars(speakerPhotos);

                    trans.Commit();
                }

                stopwatch.Stop();
                Console.WriteLine("AuditUpdate. Finished! Time: " + stopwatch.Elapsed.ToString("g"));
            }
            catch (Exception e)
            {
                Console.WriteLine("AuditUpdate. " + e);
                new DotNetRuLogger().Report(e);
            }
        }

        private static void UpdateSpeakerAvatars(IEnumerable<UpdatedFile> speakerPhotos)
        {
            foreach (UpdatedFile updatedFile in speakerPhotos)
            {
                var speakerID = updatedFile.Filename.Split('/')[2];

                byte[] speakerAvatar = updatedFile.Content;

                var speaker = RealmService.AuditRealm.Find<Speaker>(speakerID);
                speaker.Avatar = speakerAvatar;

                Console.WriteLine("AuditUpdate. Updated speaker avatar: " + updatedFile.Filename);
            }
        }

        private static void UpdateModels<T>(IEnumerable<UpdatedFile> xmlFiles, string entityName)
        {
            var newEntities = xmlFiles.Where(x => x.Filename.StartsWith("db/" + entityName));
            UpdateModels<T>(newEntities);
        }

        private static void UpdateModels<T>(IEnumerable<UpdatedFile> files)
        {
            foreach (UpdatedFile file in files)
            {
                using (var memoryStream = new MemoryStream(file.Content))
                {
                    var xmlEntity = new XmlSerializer(typeof(T)).Deserialize(memoryStream);

                    Console.WriteLine($"AuditUpdate: updating {file.Filename}");

                    var realmType = Mapper.Configuration.GetAllTypeMaps().Single(x => x.SourceType == typeof(T))
                        .DestinationType;

                    var realmObject = Mapper.Map(xmlEntity, typeof(T), realmType);

                    RealmService.AuditRealm.Add(realmObject as RealmObject, update: true);

                    Console.WriteLine($"AuditUpdate. Updated {file.Filename}");
                }
            }
        }

        private class UpdatedFile
        {
            public string Filename { get; set; }

            public byte[] Content { get; set; }
        }
    }
}
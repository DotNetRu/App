using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using RealmGenerator;
using LibGit2Sharp;
using DotNetRu.DataStore.Audit.RealmModels;
using System.Linq;
using Realms;
using AutoMapper;
using RealmGenerator.Entities;

namespace DotNetRu.Azure
{
    public static class RealmGeneratorFunction
    {
        [FunctionName("Realm")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string commitHash = req.Query["commitHash"];

            return new FileContentResult(GenerateRealm(commitHash), "application/octet-stream")
            {
                FileDownloadName = "Audit.realm"
            };
        }

        public static byte[] GenerateRealm(string commitHash)
        {
            IList<int> dummy = new List<int>();

            var checkoutMaster = commitHash == "latest";

            var auditRepoPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            Repository.Clone("https://github.com/DotNetRu/Audit.git", auditRepoPath);

            var auditVersion = new AuditVersion();

            using (var auditRepo = new Repository(auditRepoPath))
            {
                auditVersion.CommitHash = checkoutMaster ? auditRepo.Head.Commits.First().Sha : commitHash;

                var commit = auditRepo.Commits.Single(x => x.Sha == auditVersion.CommitHash);
                Commands.Checkout(auditRepo, commit);
            }

            var tempRealmFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var config = new RealmConfiguration(tempRealmFile);

            var realm = Realm.GetInstance(config);

            realm.Write(() => { realm.Add(auditVersion); });

            InitializeAudoMapper(realm, auditRepoPath);

            realm.AddEntities<SpeakerEntity, Speaker>(auditRepoPath, "speakers");
            realm.AddEntities<FriendEntity, Friend>(auditRepoPath, "friends");
            realm.AddEntities<VenueEntity, Venue>(auditRepoPath, "venues");
            realm.AddEntities<CommunityEntity, Community>(auditRepoPath, "communities");
            realm.AddEntities<TalkEntity, Talk>(auditRepoPath, "talks");
            realm.AddEntities<MeetupEntity, Meetup>(auditRepoPath, "meetups");

            realm.Dispose();

            return File.ReadAllBytes(config.DatabasePath);
        }

        private static void InitializeAudoMapper(Realm realm, string auditPath)
        {
            Mapper.Initialize(
                cfg =>
                {
                    cfg.CreateMap<SpeakerEntity, Speaker>().AfterMap(
                        (src, dest) =>
                        {
                            dest.AvatarSmall = File.ReadAllBytes(Path.Combine(auditPath, "db", "speakers", src.Id, "avatar.small.jpg"));
                            dest.AvatarURL = "https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/" + src.Id + "/avatar.jpg";
                        });
                    cfg.CreateMap<VenueEntity, Venue>();
                    cfg.CreateMap<FriendEntity, Friend>().AfterMap(
                        (src, dest) =>
                        {
                            var friendId = src.Id;

                            dest.LogoSmall = File.ReadAllBytes(Path.Combine(auditPath, "db", "friends", friendId, "logo.small.png"));
                            dest.Logo = File.ReadAllBytes(Path.Combine(auditPath, "db", "friends", friendId, "logo.png"));
                        });
                    cfg.CreateMap<CommunityEntity, Community>();
                    cfg.CreateMap<TalkEntity, Talk>().AfterMap(
                        (src, dest) =>
                        {
                            foreach (string speakerId in src.SpeakerIds)
                            {
                                var speaker = realm.Find<Speaker>(speakerId);

                                dest.Speakers.Add(speaker);
                            }
                        });
                    cfg.CreateMap<SessionEntity, Session>().AfterMap(
                        (src, dest) =>
                        {
                            dest.Talk = realm.Find<Talk>(src.TalkId);
                        });
                    cfg.CreateMap<MeetupEntity, Meetup>()
                        .ForMember(
                            dest => dest.Sessions,
                            o => o.MapFrom((src, dest, sessions, context) => context.Mapper.Map(src.Sessions, dest.Sessions)))
                        .AfterMap(
                            (src, dest) =>
                            {
                                foreach (string friendId in src.FriendIds)
                                {
                                    var friend = realm.Find<Friend>(friendId);
                                    dest.Friends.Add(friend);
                                }

                                dest.Venue = realm.Find<Venue>(src.VenueId);
                            });
                });
        }
    }
}

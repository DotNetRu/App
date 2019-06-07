using AutoMapper;
using DotNetRu.DataStore.Audit.RealmModels;
using LibGit2Sharp;
using RealmGenerator;
using RealmGenerator.Entities;
using Realms;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Conference.RealmUpdate
{
    public class Program
    {
        private static string realmPath = @"DotNetRu.DataStore.Audit/DotNetRuOffline.realm";

        public static void Main()
        {
            var directory = Directory.GetCurrentDirectory();
            var realmFullPath = $"{directory}/../../../../{realmPath}";

            Realm.DeleteRealm(new RealmConfiguration(realmFullPath));
            var realm = Realm.GetInstance(realmFullPath);

            using (var transaction = realm.BeginWrite())
            {
                PopulateRealm(realm);
                transaction.Commit();
            }
        }

        public static void PopulateRealm(Realm realm)
        {
            IList<int> dummy = new List<int>();

            var auditRepoPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            Repository.Clone("https://github.com/DotNetRu/Audit.git", auditRepoPath);

            var auditVersion = new AuditVersion();

            using (var auditRepo = new Repository(auditRepoPath))
            {
                auditVersion.CommitHash = auditRepo.Head.Commits.First().Sha;

                var commit = auditRepo.Commits.Single(x => x.Sha == auditVersion.CommitHash);
                Commands.Checkout(auditRepo, commit);
            }

            realm.Add(auditVersion);

            var mapper = InitializeAudoMapper(realm, auditRepoPath);

            realm.AddEntities<SpeakerEntity, Speaker>(Path.Combine(auditRepoPath, "db", "speakers"), mapper);
            realm.AddEntities<FriendEntity, Friend>(Path.Combine(auditRepoPath, "db", "friends"), mapper);
            realm.AddEntities<VenueEntity, Venue>(Path.Combine(auditRepoPath, "db", "venues"), mapper);
            realm.AddEntities<CommunityEntity, Community>(Path.Combine(auditRepoPath, "db", "communities"), mapper);
            realm.AddEntities<TalkEntity, Talk>(Path.Combine(auditRepoPath, "db", "talks"), mapper);
            realm.AddEntities<MeetupEntity, Meetup>(Path.Combine(auditRepoPath, "db", "meetups"), mapper);
        }

        private static IMapper InitializeAudoMapper(Realm realm, string auditPath)
        {
            var mapperConfig = new MapperConfiguration(
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

                            if (src.SeeAlsoTalkIds != null)
                            {
                                foreach (string talkId in src.SeeAlsoTalkIds)
                                {
                                    dest.SeeAlsoTalksIds.Add(talkId);
                                }
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
                            o => o.MapFrom(src => src.Sessions))
                        .AfterMap(
                            (src, dest) =>
                            {
                                if (src.FriendIds != null)
                                {
                                    foreach (string friendId in src.FriendIds)
                                    {
                                        var friend = realm.Find<Friend>(friendId);
                                        dest.Friends.Add(friend);
                                    }
                                }

                                dest.Venue = realm.Find<Venue>(src.VenueId);
                            });
                });

            return mapperConfig.CreateMapper();
        }
    }
}

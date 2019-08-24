using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNetRu.DataStore.Audit.RealmModels;
using MoreLinq;
using Octokit;
using RealmClone;
using RealmGenerator;
using RealmGenerator.Entities;
using Realms;

namespace DotNetRu.RealmUpdate
{
    public static class UpdateManager
    {
        public const int DotNetRuAppRepositoryID = 89862917;

        // TODO use async streams once available
        public static async Task<IEnumerable<TEntity>> GetXmlEntitiesAsync<TEntity>(string entityFolderName)
        {
            var timer = Stopwatch.StartNew();

            var client = new GitHubClient(new ProductHeaderValue("dotnetru-app"));

            var auditFiles = await client.Repository.Content.GetAllContents(DotNetRuAppRepositoryID, $"db/{entityFolderName}");

            var xmlEntities = await Task.WhenAll(auditFiles.Select(file => DownloadXml<TEntity>(entityFolderName, file)));

            timer.Stop();

            Console.WriteLine($"{entityFolderName}: {timer.Elapsed}");

            return xmlEntities;
        }

        private static Task<TEntity> DownloadXml<TEntity>(string entityFolderName, RepositoryContent file)
        {
            string downloadUrl;
            switch (entityFolderName)
            {
                case "speakers":
                case "friends":
                    downloadUrl = $"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/{entityFolderName}/{file.Name}/index.xml";
                    break;
                default:
                    downloadUrl = file.DownloadUrl;
                    break;
            }

            return FileHelper.DownloadEntityAsync<TEntity>(downloadUrl);
        }

        public static async Task<IEnumerable<RepositoryContent>> GetFiles(string directory)
        {
            var contentFiles = new List<RepositoryContent>();

            var client = new GitHubClient(new ProductHeaderValue("dotnetru-app"));

            var auditFiles = await client.Repository.Content.GetAllContents(DotNetRuAppRepositoryID, directory);

            foreach (var file in auditFiles)
            {
                switch (file.Type.Value)
                {
                    case ContentType.File:
                        contentFiles.Add(file);
                        break;
                    case ContentType.Dir:
                        var nestedFiles = await GetFiles($"{directory}/{file.Name}");
                        contentFiles.AddRange(nestedFiles);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return contentFiles;
        }

        public static async Task<AuditData> GetAuditData()
        {
            var mapper = InitializeAudoMapper();

            // speakers
            var xmlSpeakers = await GetXmlEntitiesAsync<SpeakerEntity>("speakers");
            var realmSpeakers = xmlSpeakers.Select(mapper.Map<Speaker>).ToArray();

            // friends
            var xmlFriends = await GetXmlEntitiesAsync<FriendEntity>("friends");
            var realmFriends = xmlFriends.Select(mapper.Map<Friend>).ToArray();

            // venues
            var xmlVenues = await GetXmlEntitiesAsync<VenueEntity>("venues");
            var realmVenues = xmlVenues.Select(mapper.Map<Venue>).ToArray();

            // communities
            var xmlCommunities = await GetXmlEntitiesAsync<CommunityEntity>("communities");
            var realmCommunities = xmlCommunities.Select(mapper.Map<Community>).ToArray();

            // talks
            var xmlTalks = await GetXmlEntitiesAsync<TalkEntity>("talks");

            var talkMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TalkEntity, Talk>().AfterMap(
                    (src, dest) =>
                    {
                        foreach (var speakerId in src.SpeakerIds)
                        {
                            var speaker = realmSpeakers.Single(s => s.Id == speakerId);
                            dest.Speakers.Add(speaker);
                        }

                        if (src.SeeAlsoTalkIds != null)
                        {
                            foreach (var talkId in src.SeeAlsoTalkIds)
                            {
                                // TODO change to TalkModel
                                dest.SeeAlsoTalksIds.Add(talkId);
                            }
                        }
                    });
            }).CreateMapper();

            var realmTalks = xmlTalks.Select(talkMapper.Map<Talk>).ToArray();

            // meetups
            var xmlMeetups = await GetXmlEntitiesAsync<MeetupEntity>("meetups");

            var meetupMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MeetupEntity, Meetup>()
                    .ForMember(
                        dest => dest.Sessions,
                        o => o.MapFrom(src => src.Sessions))
                    .AfterMap(
                        (src, dest) =>
                        {
                            if (src.FriendIds != null)
                            {
                                foreach (var friendId in src.FriendIds)
                                {
                                    var friend = realmFriends.Single(f => f.Id == friendId);
                                    dest.Friends.Add(friend);
                                }
                            }

                            dest.Venue = realmVenues.Single(venue => venue.Id == src.VenueId);
                        });
                cfg.CreateMap<SessionEntity, Session>().AfterMap(
                    (src, dest) =>
                    {
                        dest.Id = src.TalkId;
                        dest.Talk = realmTalks.Single(talk => talk.Id == src.TalkId);
                    });
            }).CreateMapper();

            var realmMeetups = xmlMeetups.Select(meetupMapper.Map<Meetup>).ToArray();

            // audit version
            var client = new GitHubClient(new ProductHeaderValue("dotnetru-app"));
            var reference = await client.Git.Reference.Get(DotNetRuAppRepositoryID, "heads/master");
            var auditVersion = new AuditVersion
            {
                CommitHash = reference.Object.Sha
            };

            return new AuditData
            {
                AuditVersion = auditVersion,
                Venues = realmVenues,
                Communities = realmCommunities,
                Friends = realmFriends,
                Meetups = realmMeetups,
                Speakers = realmSpeakers,
                Talks = realmTalks
            };
        }

        private static IMapper InitializeAudoMapper()
        {
            var mapperConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<SpeakerEntity, Speaker>().AfterMap(
                        (src, dest) =>
                        {
                            dest.AvatarSmallURL = $"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{src.Id}/avatar.small.jpg";
                            dest.AvatarURL = $"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{src.Id}/avatar.jpg";
                        });
                    cfg.CreateMap<VenueEntity, Venue>();
                    cfg.CreateMap<FriendEntity, Friend>().AfterMap(
                        (src, dest) =>
                        {
                            dest.LogoSmallURL = $"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/friends/{src.Id}/logo.small.png";
                            dest.LogoURL = $"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/friends/{src.Id}/logo.small.png";
                        });
                    cfg.CreateMap<CommunityEntity, Community>();
                });

            return mapperConfig.CreateMapper();
        }

        public static void UpdateRealm(Realm realm, AuditData auditData)
        {
            using (var transaction = realm.BeginWrite())
            {
                MoveRealmObjects(realm, new[] { auditData.AuditVersion }, x => x.CommitHash);
                MoveRealmObjects(realm, auditData.Communities, x => x.Id);
                MoveRealmObjects(realm, auditData.Friends, x => x.Id);
                MoveRealmObjects(realm, auditData.Meetups, x => x.Id);

                MoveRealmObjects(realm, auditData.Meetups.SelectMany(m => m.Sessions), x => x.Id);

                MoveRealmObjects(realm, auditData.Speakers, x => x.Id);
                MoveRealmObjects(realm, auditData.Talks, x => x.Id);
                MoveRealmObjects(realm, auditData.Venues, x => x.Id);

                transaction.Commit();
            }
        }

        public static void MoveRealmObjects<T, TKey>(Realm realm, IEnumerable<T> newObjects, Func<T, TKey> keySelector) where T : RealmObject
        {
            // TODO use primary key
            var oldObjects = realm.All<T>().ToList();

            var objectsToRemove = oldObjects.ExceptBy(newObjects, keySelector).ToList();

            foreach (var @object in objectsToRemove)
            {
                realm.Remove(@object);
            }
            foreach (var @object in newObjects)
            {
                realm.Add(@object.Clone(), update: true);
            }
        }
    }
}

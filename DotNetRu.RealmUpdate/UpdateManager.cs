using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using DotNetRu.DataStore.Audit.RealmModels;
using DotNetRu.RealmUpdate;
using Octokit;
using RealmGenerator.Entities;
using Realms;

namespace RealmGenerator
{
    public static class UpdateManager
    {
        public const int DotNetRuAppRepositoryID = 89862917;

        public static async Task<IEnumerable<TEntity>> GetXmlEntitiesAsync<TEntity>(string entityFolderName)
        {
            // TODO use async streams once available
            var xmlEntities = new List<TEntity>();

            var client = new GitHubClient(new ProductHeaderValue("dotnetru-app"));

            var auditFiles = await client.Repository.Content.GetAllContents(DotNetRuAppRepositoryID, $"db/{entityFolderName}");

            foreach (var file in auditFiles)
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

                var entity = await FileHelper.DownloadEntityAsync<TEntity>(downloadUrl);

                xmlEntities.Add(entity);
            }

            return xmlEntities;
        }

        public static void AddRange<TEntity>(this Realm realm, IEnumerable<TEntity> entities) where TEntity : RealmObject
        {
            foreach (var entity in entities)
            {
                realm.Add(entity, update: true);
            }
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
                        foreach (string speakerId in src.SpeakerIds)
                        {
                            var speaker = realmSpeakers.Single(s => s.Id == speakerId);
                            dest.Speakers.Add(speaker);
                        }

                        if (src.SeeAlsoTalkIds != null)
                        {
                            foreach (string talkId in src.SeeAlsoTalkIds)
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
                                foreach (string friendId in src.FriendIds)
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
    }
}

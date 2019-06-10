using AutoMapper;
using DotNetRu.DataStore.Audit.RealmModels;
using Octokit;
using RealmGenerator;
using RealmGenerator.Entities;
using Realms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Conference.RealmUpdate
{
    public class Program
    {
        private static string realmPath = @"DotNetRu.DataStore.Audit/DotNetRuOffline.realm";

        public const int DotNetRuAppRepositoryID = 89862917;

        public static async Task Main()
        {
            var directory = Directory.GetCurrentDirectory();
            var realmFullPath = $"{directory}/../../../../{realmPath}";

            Realm.DeleteRealm(new RealmConfiguration(realmFullPath));

            await PopulateRealm(realmFullPath);
        }

        public static async Task PopulateRealm(string realmPath)
        {
            // TODO remove
            IList<int> dummy = new List<int>();

            var mapper = InitializeAudoMapper();

            // speakers
            var xmlSpeakers = await RealmExtensions.GetXmlEntitiesAsync<SpeakerEntity>("speakers");
            var realmSpeakers = xmlSpeakers.Select(mapper.Map<Speaker>).ToArray();

            // friends
            var xmlFriends = await RealmExtensions.GetXmlEntitiesAsync<FriendEntity>("friends");
            var realmFriends = xmlFriends.Select(mapper.Map<Friend>).ToArray();

            // venues
            var xmlVenues = await RealmExtensions.GetXmlEntitiesAsync<VenueEntity>("venues");
            var realmVenues = xmlVenues.Select(mapper.Map<Venue>).ToArray();

            // communities
            var xmlCommunities = await RealmExtensions.GetXmlEntitiesAsync<CommunityEntity>("communities");
            var realmCommunities = xmlCommunities.Select(mapper.Map<Community>).ToArray();

            // talks
            var xmlTalks = await RealmExtensions.GetXmlEntitiesAsync<TalkEntity>("talks");

            var talkMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TalkEntity, Talk>().AfterMap(
                    (src, dest) =>
                    {
                        foreach (string speakerId in src.SpeakerIds)
                        {
                            var speaker = realmSpeakers.Single(s => s.Id == speakerId); // realm.Find<Speaker>(speakerId);
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
            var xmlMeetups = await RealmExtensions.GetXmlEntitiesAsync<MeetupEntity>("meetups");

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
                                    var friend = realmFriends.Single(f => f.Id == friendId); // realm.Find<Friend>(friendId);
                                    dest.Friends.Add(friend);
                                }
                            }

                            dest.Venue = realmVenues.Single(venue => venue.Id == src.VenueId); // realm.Find<Venue>(src.VenueId);
                        });
                cfg.CreateMap<SessionEntity, Session>().AfterMap(
                    (src, dest) =>
                    {
                        dest.Talk = realmTalks.Single(talk => talk.Id == src.TalkId); // realm.Find<Talk>(src.TalkId);
                    });
            }).CreateMapper();

            var realmMeetups = xmlMeetups.Select(meetupMapper.Map<Meetup>).ToArray();

            // audit version
            var client = new GitHubClient(new Octokit.ProductHeaderValue("dotnetru-app"));
            var reference = await client.Git.Reference.Get(DotNetRuAppRepositoryID, "heads/master");
            var auditVersion = new AuditVersion
            {
                CommitHash = reference.Object.Sha
            };

            // use single thread (after all awaits) to manage realm
            var realm = Realm.GetInstance(realmPath);

            using (var transaction = realm.BeginWrite())
            {
                realm.Add(auditVersion);

                realm.AddRange(realmSpeakers);
                realm.AddRange(realmFriends);
                realm.AddRange(realmVenues);
                realm.AddRange(realmCommunities);
                realm.AddRange(realmTalks);
                realm.AddRange(realmMeetups);

                transaction.Commit();
            }
        }

        private static IMapper InitializeAudoMapper()
        {
            var mapperConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<SpeakerEntity, Speaker>().AfterMap(
                        async (src, dest) =>
                        {
                            // TODO get URL from XML entity
                            var avatarSmallUrl = $"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{src.Id}/avatar.small.jpg";

                            dest.AvatarSmall = await new HttpClient().GetByteArrayAsync(avatarSmallUrl);
                            dest.AvatarURL = $"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{src.Id}/avatar.jpg";
                        });
                    cfg.CreateMap<VenueEntity, Venue>();
                    cfg.CreateMap<FriendEntity, Friend>().AfterMap(
                        async (src, dest) =>
                        {
                            var friendId = src.Id;

                            var logoSmallUrl = $"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/friends/{src.Id}/logo.small.png";
                            var logoUrl = $"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/friends/{src.Id}/logo.png";

                            dest.LogoSmall = await new HttpClient().GetByteArrayAsync(logoSmallUrl);
                            dest.Logo = await new HttpClient().GetByteArrayAsync(logoUrl);
                        });
                    cfg.CreateMap<CommunityEntity, Community>();
                });

            return mapperConfig.CreateMapper();
        }
    }
}

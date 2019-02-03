namespace DotNetRu.DataStore.Audit.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using AutoMapper;

    using DotNetRu.DataStore.Audit.RealmModels;
    using DotNetRu.DataStore.Audit.XmlEntities;
    using DotNetRu.Utils;
    using Flurl;
    using Flurl.Http;
    using PushNotifications;
    using Realms;

    public static class UpdateService
    {
        public static async Task UpdateAudit()
        {
            try
            {
                Console.WriteLine("AuditUpdate. Started updating audit");

                string currentCommitSha;
                using (var auditRealm = Realm.GetInstance("Audit.realm"))
                {
                    var auditVersion = auditRealm.All<AuditVersion>().Single();
                    currentCommitSha = auditVersion.CommitHash;
                }

                var updateContent = await "https://dotnetrupush.azurewebsites.net/api/Update"
                    .SetQueryParam("fromCommitSha", currentCommitSha)
                    .GetJsonAsync<UpdateContent>();

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                using (var auditRealm = Realm.GetInstance("Audit.realm"))
                {
                    using (var trans = auditRealm.BeginWrite())
                    {
                        var mapper = GetAutoMapper(auditRealm);

                        UpdateModels(mapper, auditRealm, updateContent.Speakers);
                        UpdateModels(mapper, auditRealm, updateContent.Friends);
                        UpdateModels(mapper, auditRealm, updateContent.Venues);
                        UpdateModels(mapper, auditRealm, updateContent.Talks);
                        UpdateModels(mapper, auditRealm, updateContent.Meetups);

                        UpdateSpeakerAvatars(auditRealm, updateContent.Photos);

                        var auditVersion = auditRealm.All<AuditVersion>().Single();

                        auditVersion.CommitHash = updateContent.LatestVersion;
                        auditRealm.Add(auditVersion, update: true);

                        trans.Commit();
                    }
                }

                stopwatch.Stop();
            }
            catch (Exception e)
            {
                DotNetRuLogger.Report(e);
            }
        }

        private static void UpdateSpeakerAvatars(Realm auditRealm, IEnumerable<string> photoURLs)
        {
            foreach (var photoURL in photoURLs)
            {
                var speakerID = photoURL.Split('/').Reverse().Skip(1).Take(1).Single();

                byte[] speakerAvatar = new HttpClient().GetByteArrayAsync(photoURL).Result;

                var speaker = auditRealm.Find<Speaker>(speakerID);
                speaker.AvatarSmall = speakerAvatar;
            }
        }

        private static void UpdateModels<T>(IMapper mapper, Realm auditRealm, IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                var realmType = mapper.ConfigurationProvider.GetAllTypeMaps().Single(x => x.SourceType == typeof(T))
                    .DestinationType;

                var realmObject = mapper.Map(entity, typeof(T), realmType);

                auditRealm.Add(realmObject as RealmObject, update: true);
            }
        }

        private static IMapper GetAutoMapper(Realm auditRealm)
        {
            var config = new MapperConfiguration(
                cfg =>
                    {
                        cfg.CreateMap<SpeakerEntity, Speaker>().AfterMap(
                            (src, dest) =>
                                {
                                    var speakerID = src.Id;
                                    var existingSpeaker = auditRealm.Find<Speaker>(speakerID);
                                    if (existingSpeaker != null)
                                    {
                                        dest.AvatarSmall = existingSpeaker.AvatarSmall;
                                    }
                                });
                        cfg.CreateMap<VenueEntity, Venue>();
                        cfg.CreateMap<FriendEntity, Friend>();
                        cfg.CreateMap<CommunityEntity, Community>();
                        cfg.CreateMap<TalkEntity, Talk>().AfterMap(
                            (src, dest) =>
                                {
                                    foreach (string speakerId in src.SpeakerIds)
                                    {
                                        var speaker = auditRealm.Find<Speaker>(speakerId);

                                        dest.Speakers.Add(speaker);
                                    }
                                });
                        cfg.CreateMap<SessionEntity, Session>().AfterMap(
                            (src, dest) =>
                                {
                                    dest.Talk = auditRealm.Find<Talk>(src.TalkId);
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
                                            var friend = auditRealm.Find<Friend>(friendId);
                                            dest.Friends.Add(friend);
                                        }

                                        dest.Venue = auditRealm.Find<Venue>(src.VenueId);
                                    });
                    });

            return config.CreateMapper();            
        }
    }
}

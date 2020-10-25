using System.Linq;
using AutoMapper;
using DotNetRu.DataStore.Audit.RealmModels;
using DotNetRu.Models.XML;
using Realms;

namespace DotNetRu.RealmUpdateLibrary
{
    public static class MapperManager
    {
        internal static IMapper GetTalkMapper(Realm realm)
        {
            var talkMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TalkEntity, Talk>().AfterMap(
                    (src, dest) =>
                    {
                        foreach (var speakerId in src.SpeakerIds)
                        {
                            var speaker = realm.All<Speaker>().Single(s => s.Id == speakerId);
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
            return talkMapper;
        }

        internal static IMapper GetMeetupMapper(Realm realm)
        {
            return new MapperConfiguration(cfg =>
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
                                    var friend = realm.All<Friend>().Single(f => f.Id == friendId);
                                    dest.Friends.Add(friend);
                                }
                            }

                            if (src.VenueId != null)
                            {
                                dest.Venue = realm.All<Venue>().Single(venue => venue.Id == src.VenueId);
                            }
                        });
                cfg.CreateMap<SessionEntity, Session>().AfterMap(
                    (src, dest) =>
                    {
                        dest.Id = src.TalkId;
                        dest.Talk = realm.All<Talk>().Single(talk => talk.Id == src.TalkId);
                    });
            }).CreateMapper();
        }

        internal static IMapper GetAutoMapper()
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
                    cfg.CreateMap<CommunityEntity, Community>().AfterMap(
                        (src, dest) =>
                        {
                            dest.LogoUrl = $"https://raw.githubusercontent.com/AnatolyKulakov/SpbDotNet/master/Swag/{src.Id.ToLower()}-squared-logo-bordered/{src.Id.ToLower()}-squared-logo-br-200.png";
                        });
                });

            return mapperConfig.CreateMapper();
        }
    }
}

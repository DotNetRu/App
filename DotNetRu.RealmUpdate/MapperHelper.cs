using System.Linq;
using AutoMapper;
using DotNetRu.DataStore.Audit.RealmModels;
using DotNetRu.Models.XML;

namespace DotNetRu.RealmUpdate
{
    public static class MapperHelper
    {
        internal static IMapper GetTalkMapper(Speaker[] realmSpeakers)
        {
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
            return talkMapper;
        }

        internal static IMapper GetMeetupMapper(Friend[] realmFriends, Venue[] realmVenues, Talk[] realmTalks)
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
                    cfg.CreateMap<CommunityEntity, Community>();
                });

            return mapperConfig.CreateMapper();
        }
    }
}

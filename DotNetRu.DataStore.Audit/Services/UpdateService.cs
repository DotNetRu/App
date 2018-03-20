using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DotNetRu.DataStore.Audit.XmlEntities;
using Octokit;
using AutoMapper;
using DotNetRu.DataStore.Audit.RealmModels;

namespace DotNetRu.DataStore.Audit.Services
{
    public static class UpdateService
    {
        private const int RepositoryId = 89862917;

        private static readonly Dictionary<string, (Type xmlType, Type realmType)> TypeMapper = new Dictionary<string, (Type xmlType, Type realmType)>
        {
            ["communities"] = (typeof(CommunityEntity),typeof(Community)),
            ["friends"] = (typeof(FriendEntity), typeof(Friend)),
            ["meetups"] = (typeof(MeetupEntity), typeof(Meetup)),
            ["speakers"] = (typeof(SpeakerEntity), typeof(Speaker)),
            ["talks"] = (typeof(TalkEntity), typeof(Talk)),
            ["venues"] = (typeof(VenueEntity), typeof(Venue))
        };



        private static HttpClient _httpClient = new HttpClient();
        private static readonly string ByteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());


        public static void Init()
        {
            Mapper.Initialize(
                cfg =>
                {
                    cfg.CreateMap<SpeakerEntity, Speaker>().AfterMap(
                        (src, dest) =>
                        {
                            // dest.Avatar = AuditHelper.LoadImage("speakers", src.Id, "avatar.jpg");
                        });
                    cfg.CreateMap<VenueEntity, Venue>();
                    cfg.CreateMap<FriendEntity, Friend>().AfterMap(
                        (src, dest) =>
                        {
                            var friendId = src.Id;

                            //dest.LogoSmall = AuditHelper.LoadImage("friends", friendId, "logo.small.png");
                            //dest.Logo = AuditHelper.LoadImage("friends", friendId, "logo.png");
                        });
                    cfg.CreateMap<CommunityEntity, Community>();
                    cfg.CreateMap<TalkEntity, Talk>().AfterMap(
                        (src, dest) =>
                        {
                            foreach (string speakerId in src.SpeakerIds)
                            {
                                var speaker = RealmService.AuditRealm.Find<Speaker>(speakerId);

                                dest.Speakers.Add(speaker);
                            }
                        });
                    cfg.CreateMap<MeetupEntity, Meetup>().AfterMap(
                        (src, dest) =>
                        {
                            foreach (string talkId in src.TalkIds)
                            {
                                var talk = RealmService.AuditRealm.Find<Talk>(talkId);
                                dest.Talks.Add(talk);
                            }

                            foreach (string friendId in src.FriendIds)
                            {
                                var friend = RealmService.AuditRealm.Find<Friend>(friendId);
                                dest.Friends.Add(friend);
                            }

                            dest.Venue = RealmService.AuditRealm.Find<Venue>(src.VenueId);
                        });
                });
        }

        public static string GetAuditCurrentVersion()
        {
            Init();
            var client = new GitHubClient(new ProductHeaderValue("dotNetRu"));
            var r = client.Repository.Commit.Compare(RepositoryId, "3ddd7e73f395c0e5214aefddc912d9ac45689925", "master").Result;
            foreach (var file in r.Files)
            {
                var k = client.Repository.Content.GetAllContents(RepositoryId, file.Filename).Result.FirstOrDefault().Content; //.Trim();
                if (k.StartsWith(ByteOrderMarkUtf8))
                {
                    k = k.Remove(0, ByteOrderMarkUtf8.Length);
                }

                var typeName = file.Filename.Split('/')[1];
                if (!TypeMapper.ContainsKey(typeName)) continue;
                var xmlType = TypeMapper[typeName].xmlType;
                var realmType = TypeMapper[typeName].realmType;
                using (var reader = new StringReader(k))
                {
                    try
                    {
                        var m = new XmlSerializer(xmlType).Deserialize(reader);

                        dynamic changedObj = Convert.ChangeType(m, xmlType);



                        var realmObject = Mapper.Map<>(m);


                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            return "";
        }
    }
}
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
using DotNetRu.DataStore.Audit.Extensions;
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

        private static readonly string ByteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

        public static string GetAuditCurrentVersion()
        {
            var client = new GitHubClient(new ProductHeaderValue("dotNetRu"));
            var r = client.Repository.Commit.Compare(RepositoryId, "3ddd7e73f395c0e5214aefddc912d9ac45689925", "master").Result;
            foreach (var file in r.Files)
            {
                var k = client.Repository.Content.GetAllContents(RepositoryId, file.Filename).Result.FirstOrDefault().Content; 
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

                        var realmObject = Mapper.Map(m, xmlType, realmType);
                        var rc = 0;

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
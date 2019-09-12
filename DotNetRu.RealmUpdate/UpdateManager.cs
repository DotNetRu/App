using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotNetRu.DataStore.Audit.RealmModels;
using DotNetRu.Models.XML;
using Microsoft.Extensions.Logging;
using MoreLinq;
using Octokit;

namespace DotNetRu.RealmUpdate
{
    public static class UpdateManager
    {
        public const int DotNetRuAppRepositoryID = 89862917;

        public static async Task<AuditUpdate> GetAuditData()
        {
            var latestCommit = await GetLatestCommit();

            return await GetAuditData(latestCommit);
        }

        public static async Task<AuditUpdate> GetAuditData(string commitSha)
        {
            var client = new GitHubClient(new ProductHeaderValue("DotNetRu"));

            var treeResponse = await client.Git.Tree.GetRecursive(DotNetRuAppRepositoryID, commitSha);
            var filePaths = treeResponse.Tree.Select(x => x.Path);

            var fileUris = filePaths
                .Where(filePath => filePath.EndsWith(".xml"))
                .Select(filePath => new Uri($"https://raw.githubusercontent.com/DotNetRu/Audit/master/{filePath}"));

            var auditXmlUpdate = await DownloadFilesFromGitHub(fileUris);

            return GetAuditUpdate(auditXmlUpdate, commitSha);
        }

        private static AuditUpdate GetAuditUpdate(AuditXmlUpdate auditXmlUpdate, string commitSha)
        {
            var mapper = MapperHelper.GetAutoMapper();

            var realmSpeakers = auditXmlUpdate.Speakers.Select(mapper.Map<Speaker>).ToArray();

            var realmFriends = auditXmlUpdate.Friends.Select(mapper.Map<Friend>).ToArray();

            var realmVenues = auditXmlUpdate.Venues.Select(mapper.Map<Venue>).ToArray();

            var realmCommunities = auditXmlUpdate.Communities.Select(mapper.Map<Community>).ToArray();

            var talkMapper = MapperHelper.GetTalkMapper(realmSpeakers);
            var realmTalks = auditXmlUpdate.Talks.Select(talkMapper.Map<Talk>).ToArray();

            var meetupMapper = MapperHelper.GetMeetupMapper(realmFriends, realmVenues, realmTalks);
            var realmMeetups = auditXmlUpdate.Meetups.Select(meetupMapper.Map<Meetup>).ToArray();

            var auditVersion = new AuditVersion
            {
                CommitHash = commitSha
            };

            return new AuditUpdate
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

        private static async Task<string> GetLatestCommit()
        {
            var client = new GitHubClient(new ProductHeaderValue("dotnetru-app"));
            var reference = await client.Git.Reference.Get(DotNetRuAppRepositoryID, "heads/master");
            return reference.Object.Sha;
        }

        private static async Task<AuditXmlUpdate> DownloadFilesFromGitHub(IEnumerable<Uri> links)
        {
            var httpClient = new HttpClient();

            var streamTasks = links.Select(
                async link => new
                {
                    FileType = GetFileType(link),
                    Content = await httpClient.GetStringAsync(link)
                });
            var fileContents = await Task.WhenAll(streamTasks);

            // TODO get all dependencies
            var xmlMeetups = fileContents.Where(x => x.FileType == FileType.Meetup).Select(x => x.Content.Deserialize<MeetupEntity>());
            var xmlTalks = fileContents.Where(x => x.FileType == FileType.Talk).Select(x => x.Content.Deserialize<TalkEntity>());
            var xmlSpeakers = fileContents.Where(x => x.FileType == FileType.Speaker).Select(x => x.Content.Deserialize<SpeakerEntity>());
            var xmlFriends = fileContents.Where(x => x.FileType == FileType.Friend).Select(x => x.Content.Deserialize<FriendEntity>());
            var xmlVenues = fileContents.Where(x => x.FileType == FileType.Venue).Select(x => x.Content.Deserialize<VenueEntity>());
            var xmlCommunities = fileContents.Where(x => x.FileType == FileType.Community).Select(x => x.Content.Deserialize<CommunityEntity>());

            return new AuditXmlUpdate()
            {
                Speakers = xmlSpeakers,
                Friends = xmlFriends,
                Venues = xmlVenues,
                Communities = xmlCommunities,
                Talks = xmlTalks,
                Meetups = xmlMeetups
            };
        }

        private static FileType GetFileType(Uri link)
        {
            switch (link.ToString())
            {
                case var str when new Regex(@".*meetups.*").IsMatch(str):
                    return FileType.Meetup;
                case var str when new Regex(@".*talks.*").IsMatch(str):
                    return FileType.Talk;
                case var str when new Regex(@".*speakers.*").IsMatch(str):
                    return FileType.Speaker;
                case var str when new Regex(@".*friends.*").IsMatch(str):
                    return FileType.Friend;
                case var str when new Regex(@".*venues.*").IsMatch(str):
                    return FileType.Venue;
                case var str when new Regex(@".*communities.*").IsMatch(str):
                    return FileType.Community;
            }

            throw new InvalidOperationException("Unknown URL provided");
        }

        public static async Task<AuditUpdate> GetAuditUpdate(string fromCommitSha, ILogger logger)
        {
            var client = new GitHubClient(new ProductHeaderValue("DotNetRu"));

            var reference = await client.Git.Reference.Get(DotNetRuAppRepositoryID, "heads/master");
            var latestMasterCommitSha = reference.Object.Sha;

            var timer = Stopwatch.StartNew();

            // TODO handle deletions
            var contentUpdate = await client.Repository.Commit.Compare(
                                    DotNetRuAppRepositoryID,
                                    fromCommitSha,
                                    latestMasterCommitSha);
            var fileUrls = contentUpdate.Files.Where(x => x.Filename.EndsWith(".xml")).Select(file => new Uri(file.RawUrl));

            timer.Stop();
            logger.LogInformation("Getting file URLs time {GetFileUrlsTime}", timer.Elapsed);

            timer.Restart();
            var auditXmlUpdate = await DownloadFilesFromGitHub(fileUrls);
            timer.Stop();
            logger.LogInformation("Downloading files from GitHub time {DownloadFileTime}", timer.Elapsed);

            timer.Restart();

            var missingFiles = GetDependencies(auditXmlUpdate);
            var missingXmlUpdate = await DownloadFilesFromGitHub(missingFiles);

            auditXmlUpdate = auditXmlUpdate.Concat(missingXmlUpdate);

            timer.Stop();
            logger.LogInformation("Downloading missing files from GitHub time {DownloadMissingFileTime}", timer.Elapsed);

            var latestCommit = await GetLatestCommit();

            return GetAuditUpdate(auditXmlUpdate, latestCommit);
        }

        private static IEnumerable<Uri> GetDependencies(AuditXmlUpdate auditXmlUpdate)
        {
            var missingSpeakers = auditXmlUpdate.Talks.SelectMany(talk => GetMissingSpeakers(auditXmlUpdate, talk));

            var missingFriends = auditXmlUpdate.Meetups.SelectMany(meetup => GetMissingFriends(auditXmlUpdate, meetup));

            var missingVenues = auditXmlUpdate.Meetups.SelectMany(meetup => GetMissingVenues(auditXmlUpdate, meetup));

            var missingTalks = auditXmlUpdate.Meetups.SelectMany(meetup => GetMissingTalks(auditXmlUpdate, meetup));

            // TODO talk -> speakers

            return missingSpeakers
                .Concat(missingFriends)
                .Concat(missingVenues)
                .Concat(missingTalks);
        }

        private static IEnumerable<Uri> GetMissingVenues(AuditXmlUpdate auditXmlUpdate, MeetupEntity meetup)
        {
            var isNewVenue = auditXmlUpdate.Venues.Any(venue => venue.Id == meetup.VenueId);

            return isNewVenue ?
                Enumerable.Empty<Uri>() :
                new Uri[] { new Uri($"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/venues/{meetup.VenueId}.xml") };
        }

        private static IEnumerable<Uri> GetMissingTalks(AuditXmlUpdate auditXmlUpdate, MeetupEntity meetup)
        {
            var existingTalks = meetup.Sessions
                .Select(session => session.TalkId)
                .ExceptBy(auditXmlUpdate.Talks.Select(talk => talk.Id), x => x);

            var talkUrls = existingTalks.Select(talk =>
                new Uri($"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/talks/{talk}.xml"));
            return talkUrls;
        }

        private static IEnumerable<Uri> GetMissingFriends(AuditXmlUpdate auditXmlUpdate, MeetupEntity meetup)
        {
            var existingFriends = meetup.FriendIds.ExceptBy(auditXmlUpdate.Friends.Select(friend => friend.Id), x => x);

            var friendUrls = existingFriends.Select(friend =>
                new Uri($"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/friends/{friend}/index.xml"));
            return friendUrls;
        }

        private static IEnumerable<Uri> GetMissingSpeakers(AuditXmlUpdate auditXmlUpdate, TalkEntity talk)
        {
            var existingSpeakers = talk.SpeakerIds.ExceptBy(auditXmlUpdate.Speakers.Select(speaker => speaker.Id), x => x);

            var speakerUrls = existingSpeakers.Select(speaker =>
                new Uri($"https://raw.githubusercontent.com/DotNetRu/Audit/master/db/speakers/{speaker}/index.xml"));
            return speakerUrls;
        }
    }
}

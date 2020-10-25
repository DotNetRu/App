using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotNetRu.Models.XML;
using Microsoft.Extensions.Logging;
using Octokit;

namespace DotNetRu.RealmUpdateLibrary
{
    public static class UpdateManager
    {
        public const int DotNetRuAppRepositoryID = 89862917;

        public static async Task<AuditXmlUpdate> GetAuditData()
        {
            var latestCommit = await GetLatestCommit();

            return await GetAuditData(latestCommit);
        }

        public static async Task<AuditXmlUpdate> GetAuditData(string commitSha)
        {
            var client = new GitHubClient(new ProductHeaderValue("DotNetRu"));

            var treeResponse = await client.Git.Tree.GetRecursive(DotNetRuAppRepositoryID, commitSha);
            var filePaths = treeResponse.Tree.Select(x => x.Path);

            var fileUris = filePaths
                .Where(filePath => filePath.EndsWith(".xml", StringComparison.InvariantCulture))
                .Select(filePath => new Uri($"https://raw.githubusercontent.com/DotNetRu/Audit/master/{filePath}"));

            var auditXmlUpdate = await DownloadFilesFromGitHub(fileUris);

            return auditXmlUpdate;
        }

        public static async Task<AuditXmlUpdate> GetAuditUpdate(string fromCommitSha, ILogger logger)
        {
            var client = new GitHubClient(new ProductHeaderValue("DotNetRu"));

            var reference = await client.Git.Reference.Get(DotNetRuAppRepositoryID, "heads/master");
            var latestMasterCommitSha = reference.Object.Sha;

            var timer = Stopwatch.StartNew();

            // TODO handle deletions
            // https://developer.github.com/v3/repos/commits/#compare-two-commits
            var contentUpdate = await client.Repository.Commit.Compare(
                                    DotNetRuAppRepositoryID,
                                    fromCommitSha,
                                    latestMasterCommitSha);
            var fileUrls = contentUpdate.Files
                .Where(x => x.Filename.EndsWith(".xml", StringComparison.InvariantCulture))
                .Select(file => new Uri(file.RawUrl));

            timer.Stop();
            logger.LogInformation("Getting file URLs time {GetFileUrlsTime}", timer.Elapsed);

            timer.Restart();
            var auditXmlUpdate = await DownloadFilesFromGitHub(fileUrls);
            timer.Stop();
            logger.LogInformation("Downloading files from GitHub time {DownloadFileTime}", timer.Elapsed);

            var latestCommit = await GetLatestCommit();

            auditXmlUpdate.FromCommitSha = fromCommitSha;
            auditXmlUpdate.ToCommitSha = latestCommit;

            return auditXmlUpdate;
        }

        private static async Task<string> GetLatestCommit()
        {
            var client = new GitHubClient(new ProductHeaderValue("dotnetru-app"));
            var reference = await client.Git.Reference.Get(DotNetRuAppRepositoryID, "heads/master");
            return reference.Object.Sha;
        }

        private static async Task<AuditXmlUpdate> DownloadFilesFromGitHub(IEnumerable<Uri> links)
        {
            using var httpClient = new HttpClient();

            var streamTasks = links.Select(
                async link => new
                {
                    FileType = GetFileType(link),
                    Content = await httpClient.GetStringAsync(link)
                });
            var fileContents = await Task.WhenAll(streamTasks);

            var xmlMeetups = fileContents.Where(x => x.FileType == FileType.Meetup).Select(x => x.Content.Deserialize<MeetupEntity>());
            var xmlTalks = fileContents.Where(x => x.FileType == FileType.Talk).Select(x => x.Content.Deserialize<TalkEntity>());
            var xmlSpeakers = fileContents.Where(x => x.FileType == FileType.Speaker).Select(x => x.Content.Deserialize<SpeakerEntity>());
            var xmlFriends = fileContents.Where(x => x.FileType == FileType.Friend).Select(x => x.Content.Deserialize<FriendEntity>());
            var xmlVenues = fileContents.Where(x => x.FileType == FileType.Venue).Select(x => x.Content.Deserialize<VenueEntity>());
            var xmlCommunities = fileContents.Where(x => x.FileType == FileType.Community).Select(x => x.Content.Deserialize<CommunityEntity>());

            return new AuditXmlUpdate
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
    }
}

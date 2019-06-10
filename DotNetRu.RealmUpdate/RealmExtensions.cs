using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Conference.RealmUpdate;
using Octokit;
using Realms;

namespace RealmGenerator
{
    public static class RealmExtensions
    {
        public static async Task<IEnumerable<TEntity>> GetXmlEntitiesAsync<TEntity>(string entityFolderName)
        {
            // TODO use async streams once available
            var xmlEntities = new List<TEntity>();

            var client = new GitHubClient(new ProductHeaderValue("dotnetru-app"));

            var auditFiles = await client.Repository.Content.GetAllContents(Program.DotNetRuAppRepositoryID, $"db/{entityFolderName}");

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

            var auditFiles = await client.Repository.Content.GetAllContents(Program.DotNetRuAppRepositoryID, directory);

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
    }
}

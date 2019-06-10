using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace RealmGenerator
{
    public class FileHelper
    {
        public static async Task<T> DownloadEntityAsync<T>(string url)
        {
            var httpClient = new HttpClient();
            var xmlContent = await httpClient.GetStringAsync(url);

            var reader = new StringReader(xmlContent);
            var xmlSerializer = new XmlSerializer(typeof(T));

            return (T)xmlSerializer.Deserialize(reader);
        }
    }
}

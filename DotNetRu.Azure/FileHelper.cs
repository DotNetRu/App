namespace RealmGenerator
{
    using System.Xml;
    using System.Xml.Serialization;

    public class FileHelper
    {
        public static T LoadFromFile<T>(string path)
        {
            var xmlReader = XmlReader.Create(path);
            var xmlSerializer = new XmlSerializer(typeof(T));

            return (T)xmlSerializer.Deserialize(xmlReader);
        }
    }
}

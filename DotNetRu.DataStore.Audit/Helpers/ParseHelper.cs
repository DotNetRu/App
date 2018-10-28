namespace DotNetRu.DataStore.Audit.Helpers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Xml.Serialization;

    internal static class ParseHelper
    {
        public static List<T> ParseXml<T>(string entitiesName)
            where T : class
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetRu.DataStore.Audit"));
            var stream = assembly.GetManifestResourceStream($"DotNetRu.DataStore.Audit.Storage.{entitiesName}.xml");
            List<T> entities;
            using (var reader = new StreamReader(stream))
            {
                var xRoot = new XmlRootAttribute { ElementName = entitiesName, IsNullable = true };
                var serializer = new XmlSerializer(typeof(List<T>), xRoot);
                entities = (List<T>)serializer.Deserialize(reader);
            }

            return entities;
        }
    }
}

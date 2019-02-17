using System.IO;
using System.Xml.Serialization;

namespace PushNotifications
{
    public static class StringExtensions
    {
        public static T Deserialize<T>(this string input)
        {
            var ser = new XmlSerializer(typeof(T));

            using (var sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }
    }
}

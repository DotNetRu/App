using System.IO;
using System.Linq;
using System.Reflection;

namespace DotNetRu.Utils.Helpers
{
    public static class ResourceHelper
    {
        public static byte[] ExtractResource(string resourceName)
        {
            var assembly = Assembly.GetCallingAssembly();

            var resourceFullName = assembly.GetManifestResourceNames().Single(x => x.Contains(resourceName));
            using (var resFilestream = assembly.GetManifestResourceStream(resourceFullName))
            {
                using (var memoryStream = new MemoryStream())
                {
                    resFilestream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}

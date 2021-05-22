using System;
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

            var resourceFullName = assembly.GetManifestResourceNames().Single(x => x.Contains(resourceName, StringComparison.InvariantCultureIgnoreCase));
            using var resFileStream = assembly.GetManifestResourceStream(resourceFullName);
            using var memoryStream = new MemoryStream();
            resFileStream?.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}

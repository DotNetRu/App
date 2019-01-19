using System.Reflection;

namespace DotNetRu.Utils.Helpers
{
    public static class ResourceHelper
    {
        public static byte[] ExtractResource(string resourceName)
        {
            var assembly = Assembly.GetCallingAssembly();
            using (var resFilestream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resFilestream == null)
                {
                    return null;
                }

                byte[] resultBytes = new byte[resFilestream.Length];
                resFilestream.Read(resultBytes, 0, resultBytes.Length);
                return resultBytes;
            }
        }
    }
}

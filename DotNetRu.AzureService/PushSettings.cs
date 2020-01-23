using System.Collections.Generic;

namespace DotNetRu.Azure
{
    public class PushSettings
    {
        public string AppType { get; set; }

        public IEnumerable<string> AppCenterAppNames { get; set; }

        public string ApiToken { get; set; }

        public string AppCenterApiToken { get; set; }
    }
}

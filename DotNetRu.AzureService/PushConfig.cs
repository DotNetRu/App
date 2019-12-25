using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetRu.Azure
{
    public class PushConfig
    {
        public string AppType { get; set; }

        public IEnumerable<string> AppCenterAppNames { get; set; }

        public string ApiToken { get; set; }

        public string AppCenterApiToken { get; set; }
    }
}

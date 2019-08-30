using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace DotNetRu.Azure
{
    internal static class ConfigManager
    {
        public static PushConfig[] GetPushConfigs(ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("pushconfig.json", optional: true, reloadOnChange: true)
                .Build();

            return config
                .GetSection("PushConfig")
                .Get<PushConfig[]>();
        }
    }
}

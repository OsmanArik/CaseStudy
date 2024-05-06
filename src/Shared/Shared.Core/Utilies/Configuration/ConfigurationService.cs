using Shared.Core.Constants;
using Microsoft.Extensions.Configuration;

namespace Shared.Core.Utilies.Configuration
{
    public static class ConfigurationService
    {
        public static IConfigurationRoot GetConfiguration()
        {
            IConfigurationRoot configuration;

            if (Environment.GetEnvironmentVariable(EnvironmentVariableKeys.ASPNETCORE_ENVIRONMENT) == null)
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
            }
            else
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable(EnvironmentVariableKeys.ASPNETCORE_ENVIRONMENT)}.json")
                    .Build();
            }

            return configuration;
        }
    }
}
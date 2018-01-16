using System.IO;
using Microsoft.Extensions.Configuration;

namespace ServiceBusExamples.MessagesSender.NetCore.Web
{
    public static class ConfigurationHelper
    {
        private static IConfigurationRoot configuration;

        public static string GetServiceBusConnectionString()
        {
            GetConfigurationIfNotExists();

            return configuration.GetValue<string>("ServiceBusConnectionString");
        }

        public static string GetCosmosDbPrimaryKey()
        {
            GetConfigurationIfNotExists();

            return configuration.GetValue<string>("CosmosDBPrimaryKey");
        }

        public static string GetCosmosDbEndpointUri()
        {
            GetConfigurationIfNotExists();

            return configuration.GetValue<string>("CosmosDbEndpointUri");
        }

        private static void GetConfigurationIfNotExists()
        {
            if (configuration != null)
            {
                return;
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            configuration = builder.Build();
        } 
    }
}

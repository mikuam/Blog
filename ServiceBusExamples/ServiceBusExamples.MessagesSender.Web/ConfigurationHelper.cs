using System.IO;
using Microsoft.Extensions.Configuration;

namespace ServiceBusExamples.MessagesSender.Web
{
    public static class ConfigurationHelper
    {
        private static string connection;

        public static string ServiceBusConnectionString()
        {
            if (string.IsNullOrWhiteSpace(connection))
            {
                connection = GetServiceBusConnectionString();
            }

            return connection;
        }

        private static string GetServiceBusConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            var value = config.GetValue<string>("ServiceBusConnectionString");
            return value;
        }
    }
}

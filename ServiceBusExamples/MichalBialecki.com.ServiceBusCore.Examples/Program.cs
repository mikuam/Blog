using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using MichalBialecki.com.ServiceBusCore.Examples.AccountTransfer;

namespace MichalBialecki.com.ServiceBusCore.Examples
{
    class Program
    {
        private static IConfigurationRoot configuration;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            configuration = builder.Build();

            var s = new Stopwatch();
            s.Start();

            var service = new AccountTransferService(configuration);

            Console.WriteLine("Starting...");
            //service.Run();
            /*
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var manager = new ServiceBusManager();

            manager.GetOrCreateTopicSubscription(
                configuration["ServiceBusConnectionString"],
                "balanceUpdates",
                "saveSubscription").GetAwaiter().GetResult();
                */
            (new MessageSender().Send()).GetAwaiter().GetResult();

            //new MessageReceiver().Receive();

            //new MessageProcessor().Process();

            //(new MessageReceiver()).ReceiveAll();

            //(new MessageSender()).Send1000().GetAwaiter().GetResult();

            s.Stop();

            //Console.WriteLine("Sent in: " + s.Elapsed);

            //Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}

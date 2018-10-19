using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;

namespace MichalBialecki.com.ServiceBusCore.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var manager = new ServiceBusManager();

            manager.GetOrCreateQueue(configuration["ServiceBusConnectionString"], "createTest").GetAwaiter().GetResult();

            //(new MessageSender().Send()).GetAwaiter().GetResult();

            //new MessageReceiver().Receive();

            //new MessageProcessor().Process();

            var s = new Stopwatch();
            s.Start();
            
            //(new MessageReceiver()).ReceiveAll();

            //(new MessageSender()).Send1000().GetAwaiter().GetResult();

            s.Stop();

            Console.WriteLine("Sent in: " + s.Elapsed);

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}

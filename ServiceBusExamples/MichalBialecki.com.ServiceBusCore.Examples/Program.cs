using System;
using System.Diagnostics;

namespace MichalBialecki.com.ServiceBusCore.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            //(new MessageSender().Send()).GetAwaiter().GetResult();

            //new MessageReceiver().Receive();

            //new MessageProcessor().Process();

            var s = new Stopwatch();
            s.Start();
            
            (new MessageReceiver()).ReceiveAll();

            //(new MessageSender()).Send1000().GetAwaiter().GetResult();

            s.Stop();

            Console.WriteLine("Sent in: " + s.Elapsed);

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}

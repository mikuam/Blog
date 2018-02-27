using System;

namespace MichalBialecki.com.ServiceBusCore.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            new MessageReceiver().Receive();

            Console.ReadKey();
        }
    }
}

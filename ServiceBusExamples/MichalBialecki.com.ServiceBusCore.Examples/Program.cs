﻿using System;

namespace MichalBialecki.com.ServiceBusCore.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            //(new MessageSender().Send()).GetAwaiter().GetResult();

            new MessageReceiver().Receive();

            Console.ReadKey();
        }
    }
}
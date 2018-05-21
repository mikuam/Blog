using MichalBialecki.com.ServiceBus.Examples;

namespace ServiceBusExamples
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                (new MessageReceiverOld().ReceiveOne()).GetAwaiter().GetResult();
                //new AccountTransferExample().Process();

                //SimpleExamples.SimpleAndSmartSendBatch();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            Console.ReadKey();
        }
    }
}

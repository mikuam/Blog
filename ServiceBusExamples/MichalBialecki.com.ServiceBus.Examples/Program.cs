namespace ServiceBusExamples
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new AccountTransferExample().Process();

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

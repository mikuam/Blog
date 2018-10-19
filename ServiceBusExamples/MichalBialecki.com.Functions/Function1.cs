using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace MichalBialecki.com.Functions
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([ServiceBusTrigger("function_testing", Connection = "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=")]string myQueueItem, TraceWriter log)
        {
            log.Info($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}

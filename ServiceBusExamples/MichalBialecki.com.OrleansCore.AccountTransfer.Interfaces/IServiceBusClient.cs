using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace MichalBialecki.com.OrleansCore.AccountTransfer.Interfaces
{
    public interface IServiceBusClient
    {
        Task SendMessageAsync(Message message);
    }
}
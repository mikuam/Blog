using System;
using System.Threading.Tasks;
using Orleans;

namespace MichalBialecki.com.OrleansCore.AccountTransfer.Interfaces
{
    public interface IATMGrain : IGrainWithIntegerKey
    {
        [Transaction(TransactionOption.RequiresNew)]
        Task Transfer(int fromAccount, int toAccount, decimal amountToTransfer);
    }
}

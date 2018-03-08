using System;
using System.Threading.Tasks;
using MichalBialecki.com.OrleansCore.AccountTransfer.Interfaces;
using Orleans;
using Orleans.Concurrency;

namespace MichalBialecki.com.OrleansCore.AccountTransfer.Grains
{
    [StatelessWorker]
    public class ATMGrain : Grain, IATMGrain
    {
        Task IATMGrain.Transfer(int fromAccount, int toAccount, decimal amountToTransfer)
        {
            return Task.WhenAll(
                this.GrainFactory.GetGrain<IAccountGrain>(fromAccount).Withdraw(amountToTransfer),
                this.GrainFactory.GetGrain<IAccountGrain>(toAccount).Deposit(amountToTransfer));
        }
    }
}

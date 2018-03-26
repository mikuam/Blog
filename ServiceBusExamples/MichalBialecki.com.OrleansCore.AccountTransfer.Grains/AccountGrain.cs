using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.CodeGeneration;
using MichalBialecki.com.OrleansCore.AccountTransfer.Interfaces;
using Microsoft.Azure.ServiceBus;
using System.Text;
using Newtonsoft.Json;

[assembly: GenerateSerializer(typeof(MichalBialecki.com.OrleansCore.AccountTransfer.Grains.Balance))]

namespace MichalBialecki.com.OrleansCore.AccountTransfer.Grains
{
    [Serializable]
    public class Balance
    {
        public decimal Value { get; set; } = 0;
    }
    
    public class AccountGrain : Grain<Balance>, IAccountGrain
    {
        private readonly IServiceBusClient serviceBusClient;

        public AccountGrain(IServiceBusClient serviceBusClient)
        {
            this.serviceBusClient = serviceBusClient;
        }

        async Task IAccountGrain.Deposit(decimal amount)
        {
            try
            {
                this.State.Value += amount;
                await this.WriteStateAsync();

                await NotifyBalanceUpdate();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        async Task IAccountGrain.Withdraw(decimal amount)
        {
            this.State.Value -= amount;
            await this.WriteStateAsync();

            await NotifyBalanceUpdate();
        }

        Task<decimal> IAccountGrain.GetBalance()
        {
            return Task.FromResult(this.State.Value);
        }

        private async Task NotifyBalanceUpdate()
        {
            var balanceUpdate = new BalanceUpdateMessage
            {
                AccountNumber = (int)this.GetPrimaryKeyLong(),
                Balance = this.State.Value
            };

            var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(balanceUpdate)));
            await serviceBusClient.SendMessageAsync(message);
        }
    }
}

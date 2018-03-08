using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.CodeGeneration;
using Orleans.Transactions.Abstractions;
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

    public class AccountGrain : Grain, IAccountGrain
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://bialecki.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=39cH/mE4siF49REMd9xtjVlUwoc0yPJNz9J8isRc9vY=";

        private readonly ITransactionalState<Balance> balance;

        private TopicClient topicClient;

        public AccountGrain(
            [TransactionalState("balance")] ITransactionalState<Balance> balance)
        {
            this.balance = balance ?? throw new ArgumentNullException(nameof(balance));
            topicClient = new TopicClient(ServiceBusConnectionString, "balanceUpdates");
        }

        async Task IAccountGrain.Deposit(decimal amount)
        {
            this.balance.State.Value += amount;
            this.balance.Save();

            await NotifyBalanceUpdate();
        }

        async Task IAccountGrain.Withdraw(decimal amount)
        {
            this.balance.State.Value -= amount;
            this.balance.Save();

            await NotifyBalanceUpdate();
        }

        Task<decimal> IAccountGrain.GetBalance()
        {
            return Task.FromResult(this.balance.State.Value);
        }

        private async Task NotifyBalanceUpdate()
        {
            var balanceUpdate = new BalanceUpdateMessage
            {
                AccountNumber = (int)this.GetPrimaryKeyLong(),
                Balance = this.balance.State.Value
            };

            var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(balanceUpdate)));
            await topicClient.SendAsync(message);
        }
    }
}

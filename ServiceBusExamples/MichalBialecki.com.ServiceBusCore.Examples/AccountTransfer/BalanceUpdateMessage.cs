namespace MichalBialecki.com.ServiceBusCore.Examples.AccountTransfer
{
    public class BalanceUpdateMessage
    {
        public int AccountNumber { get; set; }

        public decimal Balance { get; set; }
    }
}

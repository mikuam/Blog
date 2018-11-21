namespace MichalBialecki.com.ServiceBusCore.Examples.AccountTransfer
{
    public class AccountTransferMessage
    {
        public int From { get; set; }

        public int To { get; set; }

        public decimal Amount { get; set; }
    }
}

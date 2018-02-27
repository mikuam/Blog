namespace MichalBialecki.com.ServiceBusCore.Examples
{
    public class ProductRatingUpdateMessage
    {
        public int ProductId { get; set; }

        public int SellerId { get; set; }

        public int RatingSum { get; set; }

        public int RatingCount { get; set; }
    }
}

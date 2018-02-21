namespace MichalBialecki.com.OrleansCore.ProductsHost
{
    public class ProductRatingUpdateMessage
    {
        public int ProductId { get; set; }

        public int RatingSum { get; set; }

        public int RatingCount { get; set; }
    }
}

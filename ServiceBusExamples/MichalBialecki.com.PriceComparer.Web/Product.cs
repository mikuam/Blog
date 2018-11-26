using System.Collections.Generic;

namespace MichalBialecki.com.PriceComparer.Web
{
    public class Product
    {
        public Product()
        {
            Offers = new List<SellerOffer>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public List<SellerOffer> Offers { get; set; }
    }
}

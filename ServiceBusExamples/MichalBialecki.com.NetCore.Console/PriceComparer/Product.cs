using System.Collections.Generic;

namespace MichalBialecki.com.NetCore.Console.PriceComparer
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

using System;
using System.Collections.Generic;

namespace MichalBialecki.com.NetCore.Console.PriceComparer
{
    public class Seller
    {
        public Seller()
        {
            Offers = new List<Offer>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public int MarksCount { get; set; }

        public decimal MarksSum { get; set; }

        public decimal Rating => Math.Round(MarksSum / (MarksCount == 0 ? 1 : MarksCount), 2);

        public List<Offer> Offers { get; set; }
    }
}

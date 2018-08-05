using System;

namespace MichalBialecki.com.NetCore.Web.CsvExport
{
    public class ProductDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ReferenceNumber { get; set; }

        public string ProducerName { get; set; }

        public int QuantityAvailable { get; set; }

        public int QuantitySoldLastMonth { get; set; }

        public decimal Weight { get; set; }

        public decimal Price { get; set; }

        public DateTime LastOrderDate { get; set; }
    }
}

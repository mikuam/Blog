using System;
using MichalBialecki.com.NetCore.Web.CsvExport.Attributes;

namespace MichalBialecki.com.NetCore.Web.CsvExport.Data
{
    public class ProductDto
    {
        [ProductComparerExport(ExportName = "InternalId")]
        [ProductAnalytics(Order = 1)]
        public string Id { get; set; }

        [ProductAnalytics(Order = 3)]
        public string Name { get; set; }

        [ProductComparerExport(ExportName = "Id")]
        [ProductAnalytics(Order = 2)]
        public string ReferenceNumber { get; set; }

        [ProductComparerExport]
        [ProductAnalytics(Order = 4)]
        public string ProducerName { get; set; }

        [ProductAnalytics(Order = 5)]
        public int QuantityAvailable { get; set; }

        [ProductAnalytics(Order = 6)]
        public int QuantitySoldLastMonth { get; set; }

        [ProductComparerExport(Format = "0.0")]
        public decimal Weight { get; set; }

        [ProductComparerExport(Format = "0.00")]
        [ProductAnalytics(Order = 7, Format = "0.00")]
        public decimal Price { get; set; }

        [ProductComparerExport(ExportName = "OrderDate", Format = "yyyy-MM-dd")]
        [ProductAnalytics(Order = 8, Format = "MM-dd-yyyy")]
        public DateTime LastOrderDate { get; set; }
    }
}

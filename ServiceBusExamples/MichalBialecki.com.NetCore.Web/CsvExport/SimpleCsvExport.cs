using MichalBialecki.com.NetCore.Web.CsvExport.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MichalBialecki.com.NetCore.Web.CsvExport
{
    public class SimpleCsvExport : ICsvExport
    {
        public string ReturnData(IList<ProductDto> products)
        {
            var columnNames = GetColumnNames();
            var builder = new StringBuilder();

            builder.AppendJoin(";", columnNames);
            builder.AppendLine();

            foreach (var product in products)
            {
                var values = GetValues(product);
                builder.AppendJoin(";", values);
                builder.AppendLine();
            }

            return builder.ToString();
        }

        private string[] GetColumnNames()
        {
            return new[] {
            "Id",
            "Name",
            "ReferenceNumber",
            "ProducerName",
            "QuantityAvailable",
            "QuantitySoldLastMonth",
            "Weight",
            "Price",
            "LastOrderDate"};
        }

        private string[] GetValues(ProductDto product)
        {
            return new[]
            {
                product.Id,
                product.Name,
                product.ReferenceNumber,
                product.ProducerName,
                product.QuantityAvailable.ToString(),
                product.QuantitySoldLastMonth.ToString(),
                product.Weight.ToString(),
                product.Price.ToString(),
                product.LastOrderDate.ToString()
            };
        }
    }
}

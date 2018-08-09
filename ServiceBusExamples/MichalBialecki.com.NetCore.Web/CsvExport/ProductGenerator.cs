using Bogus;
using System;
using System.Collections.Generic;

namespace MichalBialecki.com.NetCore.Web.CsvExport
{
    public class ProductGenerator : IProductGenerator
    {
        public List<ProductDto> GenerateProducts(int count)
        {
            var productGenerator = new Faker<ProductDto>()
                .RuleFor(p => p.Id, v => Guid.NewGuid().ToString())
                .RuleFor(p => p.Name, v => v.Commerce.ProductName())
                .RuleFor(p => p.ReferenceNumber, v => v.IndexGlobal.ToString())
                .RuleFor(p => p.ProducerName, v => v.Company.CompanyName())
                .RuleFor(p => p.QuantityAvailable, v => v.Random.Number(0, 100))
                .RuleFor(p => p.QuantitySoldLastMonth, v => v.Random.Number(0, 20))
                .RuleFor(p => p.Weight, v => Math.Round(v.Random.Decimal(0.1m, 50), 2))
                .RuleFor(p => p.Price, v => Math.Round(v.Random.Decimal(1, 10000), 2))
                .RuleFor(p => p.LastOrderDate, v => v.Date.Recent());

            return productGenerator.Generate(count);
        }
    }
}

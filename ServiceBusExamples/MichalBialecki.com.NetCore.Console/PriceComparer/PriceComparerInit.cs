using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Newtonsoft.Json;

namespace MichalBialecki.com.NetCore.Console.PriceComparer
{
    public class PriceComparerInit
    {
        private HttpClient httpClient;
        private Faker faker;

        private const string SFUrl = "http://fab-cl-fv80j72.fabres.local:8505";

        public void Init(int numberOfSellers, int numberOfProducts)
        {
            httpClient = new HttpClient();
            faker = new Faker();

            System.Console.WriteLine("Generating sellers");
            var sellers = GenerateSellers(numberOfSellers, numberOfProducts);
            System.Console.WriteLine("Generating products");
            var products = GenerateProducts(numberOfProducts, sellers);

            System.Console.WriteLine("Init sellers started");

            var sellerTasks = sellers.Select(s => httpClient.PostAsync($"{SFUrl}/api/seller", GetJson(s)));
            Task.WaitAll(sellerTasks.ToArray());

            System.Console.WriteLine($"{sellers.Count} sellers initialized");

            System.Console.WriteLine("Init products started");

            var productTasks = products.Select(p => httpClient.PostAsync($"{SFUrl}/api/product", GetJson(p)));
            Task.WaitAll(productTasks.ToArray());

            System.Console.WriteLine($"{products.Count} products initialized");

            System.Console.WriteLine("Done");
        }

        private List<Product> GenerateProducts(int numberOfProducts, List<Seller> sellers)
        {
            var products = new List<Product>();
            for (int i = 1; i <= numberOfProducts; i++)
            {
                products.Add(new Product
                {
                    Id = i.ToString(),
                    Name = faker.Commerce.Product()
                });
            }

            var productsDict = products.ToDictionary(p => p.Id);

            foreach (var seller in sellers)
            {
                foreach (var sellerOffer in seller.Offers)
                {
                    productsDict[sellerOffer.ProductId].Offers.Add(new SellerOffer
                    {
                        ProductId = productsDict[sellerOffer.ProductId].Id,
                        Price = sellerOffer.Price,
                        SellerId = seller.Id,
                        SellerName = seller.Name,
                        SellerRating = seller.Rating
                    });
                }
            }

            products = productsDict.Values.ToList();
            foreach (var product in products)
            {
                product.Offers = product.Offers.OrderByDescending(o => o.SellerRating).ToList();
            }

            return products;
        }

        private List<Seller> GenerateSellers(int numberOfSellers, int numberOfProducts)
        {
            var random = new Random();
            var sellers = new List<Seller>();
            for (int i = 1; i <= numberOfSellers; i++)
            {
                var numberOfMarks = random.Next(1, 5);
                var marksSum = Math.Round(random.NextDouble() * 5 * numberOfMarks);

                var seller = new Seller
                {
                    Id = i.ToString(),
                    Name = faker.Company.CompanyName(),
                    MarksCount = numberOfMarks,
                    MarksSum = (decimal)marksSum
                };

                var numberOfOffers = 100;
                for (int j = 1; j <= numberOfOffers; j++)
                {
                    var productId = random.Next(1, numberOfProducts);
                    seller.Offers.Add(new Offer{ ProductId = productId.ToString(), Price = decimal.Parse(faker.Commerce.Price()) });
                }

                sellers.Add(seller);
            }
            
            return sellers;
        }

        private StringContent GetJson(object o)
        {
            return new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
        }
    }
}

using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace MichalBialecki.com.PriceComparer.Web.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private const string RemoveProduct = @"DELETE FROM Product WHERE Id = @Id";

        private const string RemoveProductOffers = @"DELETE FROM ProductOffer WHERE ProductId = @Id";

        private const string InsertProduct = @"INSERT INTO Product (Id, Name) VALUES (@Id, @Name)";

        private const string InsertProductOffer = @"INSERT INTO ProductOffer (ProductId, Price, SellerId) VALUES (@ProductId, @Price, @SellerId)";

        private const string GetProduct = @"SELECT Id, Name FROM Product WHERE Id = @id";

        private const string GetProductOffers = @"
            SELECT p.ProductId, p.Price, s.Id, s.Name, s.MarksSum / (CASE WHEN s.MarksCount = 0 THEN 1 ELSE 1 END) as SellerRating
            FROM ProductOffer p JOIN Seller s on p.SellerId = s.Id
            WHERE ProductId = @id";

        private readonly IConfigurationRoot _configuration;

        public ProductRepository(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public async Task Save(Product product)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DbConnectionString")))
            {
                await connection.ExecuteAsync(RemoveProduct, new { product.Id });
                await connection.ExecuteAsync(RemoveProductOffers, new { product.Id });

                await connection.ExecuteAsync(InsertProduct, product);
                await connection.ExecuteAsync(InsertProductOffer, product.Offers);
            }
        }

        public async Task<Product> Get(string id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DbConnectionString")))
            {
                var product = await connection.QuerySingleOrDefaultAsync<Product>(GetProduct, new { id });
                if (product == null)
                {
                    return null;
                }

                var productOffers = await connection.QueryAsync<SellerOffer>(GetProductOffers, new { id });

                product.Offers = productOffers.OrderByDescending(o => o.SellerRating).ToList();
                return product;
            }
        }
    }
}

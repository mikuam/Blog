using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace MichalBialecki.com.PriceComparer.Web.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        private const string RemoveSeller = @"DELETE FROM Seller WHERE Id = @Id";

        private const string InsertSeller = @"INSERT INTO Seller (Id, Name, MarksCount, MarksSum) VALUES (@Id, @Name, @MarksCount, @MarksSum)";

        private const string UpdateSellerRating = @"UPDATE Seller SET MarksCount = @MarksCount, MarksSum = @MarksSum WHERE Id = @Id";

        private const string GetSeller = @"SELECT Id, Name, MarksCount, MarksSum FROM Seller WHERE Id = @id";

        private const string GetSellerOffers = @"SELECT ProductId, Price FROM ProductOffer WHERE SellerId = @id";

        private readonly IConfigurationRoot _configuration;

        public SellerRepository(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public async Task Save(Seller seller)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DbConnectionString")))
            {
                await connection.ExecuteAsync(RemoveSeller, new { seller.Id });

                await connection.ExecuteAsync(InsertSeller, seller);
            }
        }

        public async Task<Seller> Get(string id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DbConnectionString")))
            {
                var sellerOffers = await connection.QueryAsync<Offer>(GetSellerOffers, new { id });
                var seller = await connection.QuerySingleAsync<Seller>(GetSeller, new { id });

                seller.Offers = sellerOffers.ToList();

                return seller;
            }
        }

        public async Task Update(Seller seller)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DbConnectionString")))
            {
                await connection.ExecuteAsync(UpdateSellerRating, seller);
            }
        }
    }
}

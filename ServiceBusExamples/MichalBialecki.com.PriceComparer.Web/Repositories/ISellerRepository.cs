using System.Threading.Tasks;

namespace MichalBialecki.com.PriceComparer.Web.Repositories
{
    public interface ISellerRepository
    {
        Task<Seller> Get(string id);
        Task Save(Seller seller);
        Task Update(Seller seller);
    }
}
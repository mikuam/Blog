using System.Threading.Tasks;

namespace MichalBialecki.com.PriceComparer.Web.Repositories
{
    public interface IProductRepository
    {
        Task<Product> Get(string id);
        Task Save(Product product);
    }
}
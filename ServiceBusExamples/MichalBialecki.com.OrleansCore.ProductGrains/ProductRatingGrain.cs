using Orleans;
using System.Threading.Tasks;

namespace MichalBialecki.com.OrleansCore.ProductGrains
{
    /// <summary>
    /// Grain implementation class Store.
    /// </summary>
    public class ProductRatingGrain : Grain, ProductGrainInterfaces.IProductRatingGrain
    {
        public async Task<float> GetRating()
        {
            return (float)4.5;
        }

        public async Task UpdateRating(float rating)
        {
            return;
        }
    }
}

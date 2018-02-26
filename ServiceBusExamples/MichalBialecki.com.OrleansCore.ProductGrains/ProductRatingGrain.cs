using Orleans;
using System.Threading.Tasks;

namespace MichalBialecki.com.OrleansCore.ProductGrains
{
    /// <summary>
    /// Grain implementation class Store.
    /// </summary>
    public class ProductRatingGrain : Grain, ProductGrainInterfaces.IProductRatingGrain
    {
        private int RatingSum { get; set; }

        private int RatingCount { get; set; }

        private int SellerId { get; set; }

        public async Task<float> GetRatingAsync()
        {
            return (float)4.5;
        }

        public async Task UpdateRatingAsync(int ratingSum, int ratingCount, int sellerId)
        {
            return;
        }
    }
}

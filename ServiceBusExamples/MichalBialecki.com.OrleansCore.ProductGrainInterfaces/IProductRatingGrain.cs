using Orleans;
using System.Threading.Tasks;

namespace MichalBialecki.com.OrleansCore.ProductGrainInterfaces
{
    public interface IProductRatingGrain : IGrainWithIntegerKey
    {
        Task<float> GetRatingAsync();

        Task UpdateRatingAsync(int ratingSum, int ratingCount, int sellerId);
    }
}

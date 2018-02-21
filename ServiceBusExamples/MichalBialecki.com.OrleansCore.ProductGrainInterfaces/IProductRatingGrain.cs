using Orleans;
using System.Threading.Tasks;

namespace MichalBialecki.com.OrleansCore.ProductGrainInterfaces
{
    public interface IProductRatingGrain : IGrainWithIntegerKey
    {
        Task<float> GetRating();

        Task UpdateRating(int ratingSum, int ratingCount, int sellerId);
    }
}

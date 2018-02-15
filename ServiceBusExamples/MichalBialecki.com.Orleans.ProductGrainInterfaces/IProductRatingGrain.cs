using System.Threading.Tasks;
using Orleans;

namespace MichalBialecki.com.Orleans.ProductGrainInterfaces
{
    /// <summary>
    /// Grain interface IGrain1
    /// </summary>
    public interface IProductRatingGrain : IGrainWithGuidKey
    {
        Task<float> GetRating();

        Task UpdateRating(float rating);
    }
}

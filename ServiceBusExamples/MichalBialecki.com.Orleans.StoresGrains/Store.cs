using System.Threading.Tasks;
using Orleans;

namespace MichalBialecki.com.Orleans.StoresGrains
{
    /// <summary>
    /// Grain implementation class Store.
    /// </summary>
    public class Store : Grain, IStore
    {
        public async Task<float> GetBalanceForToday()
        {
            return Task.FromResult(101);
        }
    }
}

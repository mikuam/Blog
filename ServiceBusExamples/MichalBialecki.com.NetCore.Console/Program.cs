using MichalBialecki.com.NetCore.Console.Users;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MichalBialecki.com.NetCore.Console.PriceComparer;

namespace MichalBialecki.com.NetCore.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //Task.Run(async () => await Measure());

            var priceComparer = new PriceComparerInit();
            priceComparer.Init(1000, 10000);

            System.Console.ReadKey();
        }

        private static async Task Measure()
        {
            var services = new UsersService();
            var stopper = new Stopwatch();
            var userIds = GetUserIds(1000);

            stopper.Start();
            await services.GetUsersSynchrnously(userIds);
            System.Console.WriteLine("Synchronous: " + stopper.Elapsed);

            stopper.Restart();
            await services.GetUsersInParallel(userIds);
            System.Console.WriteLine("In paralell: " + stopper.Elapsed);

            stopper.Restart();
            await services.GetUsersInParallelFixed(userIds);
            System.Console.WriteLine("In paralell fixed: " + stopper.Elapsed);

            stopper.Restart();
            await services.GetUsersInParallelInWithBatches(userIds);
            System.Console.WriteLine("In paralell with batches: " + stopper.Elapsed);
        }

        private static IEnumerable<int> GetUserIds(int numberOfIds)
        {
            for (int i = 1; i <= numberOfIds; i++)
            {
                yield return i;
            }
        }
    }
}

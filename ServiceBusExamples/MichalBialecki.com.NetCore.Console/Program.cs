using MichalBialecki.com.NetCore.Console.Users;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MichalBialecki.com.NetCore.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new UsersService();
            var stopper = new Stopwatch();
            var userIds = GetUserIds(10000);

            stopper.Start();
            Task.Run(async () => await services.GetUsersSynchrnously(userIds));
            System.Console.WriteLine("Synchronous: " + stopper.Elapsed);

            stopper.Restart();
            Task.Run(async () => await services.GetUsersInParallel(userIds));
            System.Console.WriteLine("In paralell: " + stopper.Elapsed);

            stopper.Restart();
            Task.Run(async () => await services.GetUsersInParallelFixed(userIds));
            System.Console.WriteLine("In paralell fixed: " + stopper.Elapsed);

            stopper.Restart();
            Task.Run(async () => await services.GetUsersInParallelInWithBatches(userIds));
            System.Console.WriteLine("In paralell with batches: " + stopper.Elapsed);

            System.Console.ReadKey();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MichalBialecki.com.NetCore.Console.Users
{
    public class UsersService
    {
        private UsersClient client;

        public UsersService()
        {
            client = new UsersClient();
        }

        public async Task<IEnumerable<UserDto>> GetUsersSynchrnously(IEnumerable<int> userIds)
        {
            var users = new List<UserDto>();
            foreach (var id in userIds)
            {
                users.Add(await client.GetUser(id));
            }

            return users;
        }

        public async Task<IEnumerable<UserDto>> GetUsersInParallel(IEnumerable<int> userIds)
        {
            var tasks = userIds.Select(id => client.GetUser(id));
            var users = await Task.WhenAll(tasks);

            return users;
        }

        public async Task<IEnumerable<UserDto>> GetUsersInParallelFixed(IEnumerable<int> userIds)
        {
            var users = new List<UserDto>();
            var batchSize = 100;
            int numberOfBatches = (int)Math.Ceiling((double)userIds.Count() / batchSize);

            for(int i = 0; i < numberOfBatches; i++)
            {
                var currentIds = userIds.Skip(i * batchSize).Take(batchSize);
                var tasks = currentIds.Select(id => client.GetUser(id));
                users.AddRange(await Task.WhenAll(tasks));
            }
            
            return users;
        }

        public async Task<IEnumerable<UserDto>> GetUsersInParallelInWithBatches(IEnumerable<int> userIds)
        {
            var tasks = new List<Task<IEnumerable<UserDto>>>();
            var batchSize = 100;
            int numberOfBatches = (int)Math.Ceiling((double)userIds.Count() / batchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var currentIds = userIds.Skip(i * batchSize).Take(batchSize);
                tasks.Add(client.GetUsers(currentIds));
            }
            
            return (await Task.WhenAll(tasks)).SelectMany(u => u);
        }
    }
}

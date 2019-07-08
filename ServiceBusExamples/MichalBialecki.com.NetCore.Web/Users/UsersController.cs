namespace MichalBialecki.com.NetCore.Web.Users
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;

    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private static Random random = new Random();

        private readonly IUsersRepository _usersRepository;

        private readonly IDistributedCache _distributedCache;

        private readonly IUserService _userService;

        public UsersController(IUsersRepository usersRepository, IUserService userService, IDistributedCache distributedCache)
        {
            _usersRepository = usersRepository;
            _userService = userService;
            _distributedCache = distributedCache;
        }
        
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            var cacheKey = $"User_{id}";
            UserDto user;
            var userBytes = await _distributedCache.GetAsync(cacheKey);
            if (userBytes == null)
            {
                user = await _usersRepository.GetUserById(id);
                userBytes = CacheHelper.Serialize(user);
                await _distributedCache.SetAsync(
                    cacheKey,
                    userBytes,
                    new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(5) });
            }

            user = CacheHelper.Deserialize<UserDto>(userBytes);
            return Json(user);
        }

        [HttpPost("GetMany")]
        public async Task<JsonResult> GetMany([FromBody]IEnumerable<int> ids)
        {
            var users = await _usersRepository.GetUsersById(ids);
            return Json(users);
        }

        [HttpPost("InsertMany")]
        public async Task<JsonResult> InsertMany(int? number = 100)
        {
            var userNames = new List<string>();
            for (int i = 0; i < number; i++)
            {
                userNames.Add(RandomString(10));
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await _usersRepository.InsertMany(userNames);

            stopwatch.Stop();
            return Json(
                new {
                    users = number,
                    time = stopwatch.Elapsed
                    });
        }

        [HttpPost("SafeInsertMany")]
        public async Task<JsonResult> SafeInsertMany(int? number = 100)
        {
            var userNames = new List<string>();
            for (int i = 0; i < number; i++)
            {
                userNames.Add(RandomString(10));
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await _usersRepository.SafeInsertMany(userNames);

            stopwatch.Stop();
            return Json(
                new
                    {
                        users = number,
                        time = stopwatch.Elapsed
                    });
        }

        [HttpPost("InsertInBulk")]
        public async Task<JsonResult> InsertInBulk(int? number = 100)
        {
            var userNames = new List<string>();
            for (int i = 0; i < number; i++)
            {
                userNames.Add(RandomString(10));
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await _usersRepository.InsertInBulk(userNames);

            stopwatch.Stop();
            return Json(
                new
                    {
                        users = number,
                        time = stopwatch.Elapsed
                    });
        }

        [HttpGet("GetCountByCountryCode")]
        public async Task<JsonResult> GetTop100ByCountryCode()
        {
            var watch = new Stopwatch();
            watch.Start();

            var count1 = await _usersRepository.GetCountByCountryCode("PL");
            watch.Stop();
            var elapsedCount1 = watch.Elapsed;
            watch.Reset();
            watch.Start();

            var count2 = await _usersRepository.GetCountByCountryCodeAsAnsi("PL");
            watch.Stop();

            return Json(new { TimeElapsed = elapsedCount1, TimeElaspedWitAnsi = watch.Elapsed });
        }

        [HttpPost("ExportUsers")]
        public IActionResult ExportUsers()
        {
            var thread = new Thread(async () =>
            {
                var result = await _userService.ExportUsersToExternalSystem();
                if (!result)
                {
                    // log error
                }
            })
            {
                IsBackground = true
            };
            thread.Start();

            return Ok();
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

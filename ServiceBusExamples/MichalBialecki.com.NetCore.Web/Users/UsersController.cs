namespace MichalBialecki.com.NetCore.Web.Users
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private static Random random = new Random();

        private readonly IUsersRepository _usersRepository;

        private readonly IUserService _userService;

        public UsersController(IUsersRepository usersRepository, IUserService userService)
        {
            _usersRepository = usersRepository;
            _userService = userService;
        }
        
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            try
            {
                var user = await _usersRepository.GetUserById(id);
                return Json(user);
            }
            catch (Exception e)
            {
                throw;
            }
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
            Task.Run(
                async () =>
                    {
                        var result = await _userService.ExportUsersToExternalSystem();
                        if (!result)
                        {
                            // log error
                        }
                    });
            
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

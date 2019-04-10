using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MichalBialecki.com.NetCore.Web.Users
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
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
        
        [HttpPost("GenerateUsers")]
        public async Task GenerateUsers(int? number = 1000)
        {
            for (int i = 0; i < number; i++)
            {
                await _usersRepository.AddUser(RandomString(10));
            }
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

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

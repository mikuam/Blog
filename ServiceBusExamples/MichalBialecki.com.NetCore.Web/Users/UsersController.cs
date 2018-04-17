using System;
using System.Collections.Generic;
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
            var user = await _usersRepository.GetUserById(id);
            return Json(user);
        }

        [HttpPost]
        public async Task<JsonResult> Get(IEnumerable<int> ids)
        {
            var users = await _usersRepository.GetUsersById(ids);
            return Json(users);
        }
        
        [HttpPost]
        public async Task GenerateUsers(int? number = 100)
        {
            for (int i = 0; i < number; i++)
            {
                await _usersRepository.AddUser(RandomString(10));
            }
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

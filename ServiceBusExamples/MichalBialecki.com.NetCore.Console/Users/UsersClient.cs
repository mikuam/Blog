using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MichalBialecki.com.NetCore.Console.Users
{
    using System;

    public class UsersClient
    {
        private HttpClient client;

        public UsersClient()
        {
            client = new HttpClient();
        }

        public async Task<UserDto> GetUser(int id)
        {
            var response = await client.GetAsync("localhost:49532/api/users/" + id).ConfigureAwait(false);
            var user = JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync());

            return user;
        }

        public async Task<IEnumerable<UserDto>> GetUsers(IEnumerable<int> ids)
        {
            var response = await client
                //.PostAsync("http://localhost:49532/api/Users/GetMany", new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json"))
                .PostAsync(
                    "http://michalbialeckicomnetcoreweb20180417060938.azurewebsites.net/api/users/GetMany",
                    new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);

            var users = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(await response.Content.ReadAsStringAsync());

            return users;
        }

        public async Task InsertUser()
        {
            try
            {
                // async stuff here

                var response = await client.PostAsync("http://localhost:49532/api/users/InsertMany", null).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                // more async cally 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

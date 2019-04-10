using System;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MichalBialecki.com.NetCore.Web.Users
{
    public class UsersRepository : IUsersRepository
    {
        private const string ConnectionString = "Data Source=MIKLAPTOP\\SQLEXPRESS;Initial Catalog=Blog;Integrated Security=True";
        private readonly Random random = new Random();
        
        public async Task<UserDto> GetUserById(int userId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<UserDto>(
                    "SELECT [Id], [Name], [LastUpdatedAt] FROM [Users] WHERE Id = @Id",
                    new { Id = userId }).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsersById(IEnumerable<int> userIds)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return await connection.QueryAsync<UserDto>(
                    "SELECT [Id], [Name], [LastUpdatedAt] FROM [Users] WHERE Id IN @Ids",
                    new { Ids = userIds }).ConfigureAwait(false);
            }
        }

        public async Task AddUser(string name)
        {
            var countryCode = random.NextDouble() > 0.5 ? "US" : "PL";
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO [Users] (Name, CountryCode, LastUpdatedAt) VALUES (@Name, @countryCode, getdate())",
                    new { name, countryCode }).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<UserDto>> GetCountByCountryCode(string countryCode)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return await connection.QueryAsync<UserDto>(
                    "SELECT count(*) FROM [Users] WHERE CountryCode = @CountryCode",
                    new { CountryCode = countryCode }).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<UserDto>> GetCountByCountryCodeAsAnsi(string countryCode)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return await connection.QueryAsync<UserDto>(
                    "SELECT count(*) FROM [Users] WHERE CountryCode = @CountryCode",
                    new { CountryCode = new DbString() { Value = countryCode, IsAnsi = true, Length = 2 } })
                    .ConfigureAwait(false);
            }
        }
    }
}

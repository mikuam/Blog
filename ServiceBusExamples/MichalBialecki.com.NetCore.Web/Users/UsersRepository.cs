using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MichalBialecki.com.NetCore.Web.Users
{
    public class UsersRepository : IUsersRepository
    {
        //private const string ConnectionString = "Data Source=MIKLAPTOP\\SQLEXPRESS;Initial Catalog=Blog;Integrated Security=True";
        private const string ConnectionString = "Server=tcp:bialecki.database.windows.net,1433;Initial Catalog=Blog;Persist Security Info=False;User ID=michal;Password=Password1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

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
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO [Users] (Name, LastUpdatedAt) VALUES (@Name, getdate())",
                    new { name }).ConfigureAwait(false);
            }
        }
    }
}
